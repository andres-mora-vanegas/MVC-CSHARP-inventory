using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using testi2.Context;
using testi2.Models;
using static testi2.Models.SaleModels;
using static testi2.Models.Template;
using static testi2.Models.mailer;
using System.Threading.Tasks;
using System.IO;

namespace testi2.Controllers
{
    public class saleController : Controller
    {
        private bd_siugEntities db = new bd_siugEntities();
        private bd_siugEntities dba = new bd_siugEntities();

        // GET: sale
        public ActionResult Index()
        {
            ViewBag.clientId = new SelectList(db.tb_clients, "cli_id", "cli_name_company");
            ViewBag.productId = new SelectList(db.tb_stock.Where(m => m.sto_state == 1), "sto_id", "sto_descript");
            ViewBag.productQuantity = new SelectList(new List<quantity>(), "sto_quantity", "sto_quantity");
            ViewBag.productList = new SelectList(db.tb_stock.Where(m => m.sto_state == 1), "sto_id", "sto_salePrice");

            return View();
        }

        [HttpPost]
        public JsonResult stockQuantity(string id)
        {
            //obtneos el parametro
            var idStock = Int32.Parse(id);
            var obj = new JObject();
            //consultamos la cantidad disponible en la bd
            var tempi = db.tb_stock.Where(x => x.sto_id == idStock)
                        .Select(x => x.sto_avaible)
                        .FirstOrDefault();
            if (tempi >= 0)
            {
                //agregamos el numero disponible al json
                obj["state"] = "ok";
                obj["answer"] = tempi;
            }
            else
            {
                //agregamos el numero disponible al json
                obj["state"] = "error";
                obj["answer"] = "no se encontró información";
            }

            var temp = JsonConvert.SerializeObject(obj);

            //retornamos el json
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemList()
        {

            var xep = db.tb_stock.Where(x => x.sto_state.Equals(1));
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(xep);
            return Json(xep, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult sale(string id)
        {

            var bandera = "";
            var toAlert = 0;
            var productNamex = "";
            //fecha actual del sistema
            DateTime actualDate = DateTime.Now;
            String[] cultureNames = { "es-CO" };

            var obj = new JObject();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonCLass Data = serializer.Deserialize<JsonCLass>(id);

            int salePerson = Int32.Parse(Data.salePerson);
            int clientPerson = Int32.Parse(Data.clientPerson);
            //string Jsonx = Data.items.ToString();

            //validamos que los datos sean distintos a vacio
            if (salePerson != 0 && salePerson != 0)
            {
                //insertamos la venta
                var sql = new tb_sale
                {
                    sal_userId = salePerson,
                    sal_cli_id = clientPerson,
                    sal_date = actualDate,
                    sal_state = 1
                };
                db.tb_sale.Add(sql);
                db.SaveChanges();
                db.Entry(sql).GetDatabaseValues();
                //obtenemos id de la venta insertada
                var idx = sql.sal_id;

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //JsonCLass Data = serializer.Deserialize<JsonCLass>(id);

                //recorremos todos los items adquiridos
                foreach (detail objitem in Data.detail)
                {
                    //obtenemos los datos a insertar en la tabla
                    long productId = long.Parse(objitem.productId);//Get all item of jsonID .
                    string productDescription = objitem.productDescription;//Get all item of jsonID .
                    long productQuantity = long.Parse(objitem.productQuantity);//Get all item of jsonID .
                    long productSubtotal = long.Parse(objitem.productSubtotal);//Get all item of jsonID .

                    var sql2 = new tb_sale_detail
                    {
                        sade_sale_id = idx,
                        sade_stock_id = productId,
                        sade_quantity = productQuantity,
                        sade_subtotal = productSubtotal
                    };
                    //guardamos los articulos comprados
                    db.tb_sale_detail.Add(sql2);
                    db.SaveChanges();
                    //consultamos cuantos productos hay disponibles
                    var howMany = db.tb_stock.Where(x => x.sto_id == productId);
                    Decimal howManyIndividual = 0;
                    foreach (var da in howMany)
                    {
                        howManyIndividual = da.sto_avaible;
                        toAlert = da.sto_alert;
                        productNamex = da.sto_descript;
                    }
                    howManyIndividual -= productQuantity;
                    //si la cantidad de productos es mayor a 0
                    if (howManyIndividual >= 0)
                    {
                        tb_stock result = (from u in db.tb_stock
                                           where u.sto_id == productId
                                           select u).Single();
                        result.sto_avaible = Convert.ToInt32(howManyIndividual);
                    }
                    //de lo contrario inactivamos el producto
                    else
                    {
                        tb_stock result = (from u in db.tb_stock
                                           where u.sto_id == productId
                                           select u).Single();
                        result.sto_state = 2;
                        
                    }
                    //si la cantidad del producto es menor o igual a la de la alerta se enviará correo
                    if (howManyIndividual <= toAlert)
                    {
                        var obj2 = new mailerDetail();
                        obj2.mailerBody = Convert.ToString(mailTemplate(productNamex, howManyIndividual));
                        obj2.mailerSubject = "Notificación de productos";
                        obj2.mailerName = "andymora";
                        obj2.mailerEmail = "andres.mora.vanegas@outlook.com";
                        var send = new mailer();
                        bandera = send.sendEmail(obj2);
                        //bandera = JsonConvert.SerializeObject(obj2);

                    }
                    else
                    {
                        bandera = Convert.ToString(howManyIndividual + " <-cantidad disponible  alerta-> " + toAlert);
                    }

                    db.SaveChanges();
                }

                obj["state"] = "ok";
                obj["answer"] = bandera;
            }
            else
            {
                obj["state"] = "error";
                obj["answer"] = id;
            }

            var temp = JsonConvert.SerializeObject(obj);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        

        public String mailTemplate(String productName, Decimal productQuantity)
        {
            String[] pattern = new String[2];
            String[] replacement = new String[2];            

            var data = new saleNotify();
            data.productName = productName;            
            data.productQuantity = Convert.ToString(productQuantity);

            pattern[0] = "#productName#";
            pattern[1] = "#productQuantity#";            

            replacement[0] = productName;
            replacement[1] = data.productQuantity;            

            String test = viewToString("mailTemplate", data).PregReplace(pattern, replacement);            
            return test;            
        }

        public class saleNotify
        {
            public string productName { get; set; }
            public string productQuantity { get; set; }

        }

        public String viewToString(string viewName, object model)
        {

            ViewData.Model = model;
            using (var SW = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, SW);

                viewResult.View.Render(viewContext, SW);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return SW.GetStringBuilder().ToString();
            }
        }

        public string var_dump(object obj, int recursion)
        {
            StringBuilder result = new StringBuilder();

            // Protect the method against endless recursion
            if (recursion < 5)
            {
                // Determine object type
                Type t = obj.GetType();

                // Get array with properties for this object
                PropertyInfo[] properties = t.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        // Get the property value
                        object value = property.GetValue(obj, null);

                        // Create indenting string to put in front of properties of a deeper level
                        // We'll need this when we display the property name and value
                        string indent = String.Empty;
                        string spaces = "|   ";
                        string trail = "|...";

                        if (recursion > 0)
                        {
                            indent = new StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
                        }

                        if (value != null)
                        {
                            // If the value is a string, add quotation marks
                            string displayValue = value.ToString();
                            if (value is string) displayValue = String.Concat('"', displayValue, '"');

                            // Add property name and value to return string
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, displayValue);

                            try
                            {
                                if (!(value is ICollection))
                                {
                                    // Call var_dump() again to list child properties
                                    // This throws an exception if the current property value
                                    // is of an unsupported type (eg. it has not properties)
                                    result.Append(var_dump(value, recursion + 1));
                                }
                                else
                                {
                                    // 2009-07-29: added support for collections
                                    // The value is a collection (eg. it's an arraylist or generic list)
                                    // so loop through its elements and dump their properties
                                    int elementCount = 0;
                                    foreach (object element in ((ICollection)value))
                                    {
                                        string elementName = String.Format("{0}[{1}]", property.Name, elementCount);
                                        indent = new StringBuilder(trail).Insert(0, spaces, recursion).ToString();

                                        // Display the collection element name and type
                                        result.AppendFormat("{0}{1} = {2}\n", indent, elementName, element.ToString());

                                        // Display the child properties
                                        result.Append(var_dump(element, recursion + 2));
                                        elementCount++;
                                    }

                                    result.Append(var_dump(value, recursion + 1));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            // Add empty (null) property to return string
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, "null");
                        }
                    }
                    catch
                    {
                        // Some properties will throw an exception on property.GetValue()
                        // I don't know exactly why this happens, so for now i will ignore them...
                    }
                }
            }

            return result.ToString();
        }

        // GET: sale/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_sale_detail tb_sale_detail = db.tb_sale_detail.Find(id);
            if (tb_sale_detail == null)
            {
                return HttpNotFound();
            }
            return View(tb_sale_detail);
        }

        // GET: sale/Create
        public ActionResult Create()
        {
            ViewBag.sade_sale_id = new SelectList(db.tb_sale, "sal_id", "sal_id");
            ViewBag.sade_stock_id = new SelectList(db.tb_stock, "sto_id", "sto_descript");
            return View();
        }

        // POST: sale/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sade_id,sade_sale_id,sade_stock_id,sade_quantity,sade_subtotal")] tb_sale_detail tb_sale_detail)
        {
            if (ModelState.IsValid)
            {
                db.tb_sale_detail.Add(tb_sale_detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sade_sale_id = new SelectList(db.tb_sale, "sal_id", "sal_id", tb_sale_detail.sade_sale_id);
            ViewBag.sade_stock_id = new SelectList(db.tb_stock, "sto_id", "sto_descript", tb_sale_detail.sade_stock_id);
            return View(tb_sale_detail);
        }

        // GET: sale/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_sale_detail tb_sale_detail = db.tb_sale_detail.Find(id);
            if (tb_sale_detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.sade_sale_id = new SelectList(db.tb_sale, "sal_id", "sal_id", tb_sale_detail.sade_sale_id);
            ViewBag.sade_stock_id = new SelectList(db.tb_stock, "sto_id", "sto_descript", tb_sale_detail.sade_stock_id);
            return View(tb_sale_detail);
        }

        // POST: sale/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sade_id,sade_sale_id,sade_stock_id,sade_quantity,sade_subtotal")] tb_sale_detail tb_sale_detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_sale_detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sade_sale_id = new SelectList(db.tb_sale, "sal_id", "sal_id", tb_sale_detail.sade_sale_id);
            ViewBag.sade_stock_id = new SelectList(db.tb_stock, "sto_id", "sto_descript", tb_sale_detail.sade_stock_id);
            return View(tb_sale_detail);
        }

        // GET: sale/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_sale_detail tb_sale_detail = db.tb_sale_detail.Find(id);
            if (tb_sale_detail == null)
            {
                return HttpNotFound();
            }
            return View(tb_sale_detail);
        }

        // POST: sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_sale_detail tb_sale_detail = db.tb_sale_detail.Find(id);
            db.tb_sale_detail.Remove(tb_sale_detail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    internal class quantity
    {
    }
}
