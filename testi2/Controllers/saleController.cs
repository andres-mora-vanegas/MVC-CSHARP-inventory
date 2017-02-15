using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using testi2.Context;

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
        public JsonResult sale(string id) {
            var obj = new JObject();
            obj["state"] = "ok";
            obj["answer"] = id;

             var bandera = "";
            if (
            //valida que tengamos todos los datos necesarios para generar la factura, el nombre del usuario, el id del vendedor, los productos escogidos.
                    isset($_POST['cliId']) && !empty($_POST['cliId']) &&
                    isset($_POST['idUser']) && !empty($_POST['idUser']) &&
                    isset($_POST['json']) && !empty($_POST['json'])
            )
            {
                @$fecha = date("y-m-d");
                $cliId = $_POST['cliId'];
                $jsonArray = json_decode($_POST['json']);
                $idUser = $_POST['idUser'];

                //inserta la venta con el nombre del usuario, id del vendedor
                $saleInsert = $sale->create(
                        $sale->clientId = $cliId, $sale->saleUserId = $idUser
                );               
                
                $querySale = $dbConection->ejecutar($saleInsert);
                if ($querySale) {
                    //echo "<script>alert('".$jsonArray."');</script>";
                    //valida si se inserto la venta
                    $var = "1";
                    $idInsertSale = $dbConection->lastID();
                    //obtiene el id de esa venta generada
                    $saleDetailx = array();
                    $canti = count($jsonArray->arrayProduct);
                    //obtiene la cantidad de productos comprados
                    //echo "<script>alert('holaaa".$_POST['json']."');</script>";
                    $totalSale = '';
                    //foreach($jsonArray as $obj):
                    for ($x = 0; $x < $canti; $x++) {

                        //valida la cantidad de productos adquiridos
                        $productx = $jsonArray->arrayProduct[$x]->product;
                        $quantity = $jsonArray->arrayProduct[$x]->quantity;
                        $subtotal = $jsonArray->arrayProduct[$x]->subtotal;
                        $saleDetailn = $saleDetail->create($saleDetail->saleId = $idInsertSale, $saleDetail->stockId = $productx, $saleDetail->quantity = $quantity, $saleDetail->subtotal = $subtotal
                        );
                        //inserta un registro por cada producto en la tabla detalle venta							
                        $querySaleDetail = $dbConection->ejecutar($saleDetailn);
                        $stockRemaining = $product->showOne($product->id = $productx);
                        $queryStockRemaining = $dbConection->mostrar_fila($stockRemaining);
                        $bandera71 = '';
                        if ($queryStockRemaining) {
                            if ($queryStockRemaining > 0) {
                                foreach ($queryStockRemaining as $rowRemaining):
                                    $bandera71 = $rowRemaining['sto_avaible'];
                                endforeach;
                            }
                            else {
                                echo "No se encontraron productos";
                            }
                        }
                        //se resta el producto del inventario
                        $bandera71 = $bandera71 - $quantity;
                        $stockRemaining = $product->updateQuantity($product->id = $productx, $product->avaible = $bandera71);
                        $queryStockRemaining = $dbConection->ejecutar($stockRemaining);
                        //si se encontró un error disminuir los productos del inventari
                        if (!$queryStockRemaining) {
                            echo "<script>alert('error al restar el producto del inventario');</script>";
                        } else {
                            if ($bandera71 < 3) {
                                //si hay menos de 3 productos se comienza a crear el email
                                @$obj->body0.= "<tr><td>Se agotando el producto </td><td> {$rowRemaining['sto_descript']} </td></tr><tr> <td>Solo quedan </td><td>{$bandera71}</td></tr>";
                                $obj->emailProcess = true;
                            }
                            $totalSale = $totalSale + $saleDetail->subtotal = $subtotal;
                            //obtiene el valor total de la factura
                        }
                    }
                    //se valida que haya información para el email
                    if (isset($obj->emailProcess) and $obj->emailProcess == true) {
                        $obj->name = "Juan Andres Diaz";
                        $obj->email = "andymora1907@hotmail.com";
                        $obj->subject = "Se está quedando sin inventario ";
                        $obj->body = customMail($obj);
                        $obj->emailSend = sendEmail($obj);
                    }
                    //inserta los datos de la factura es decir el valor total de la venta y el id de la venta
                    $billx = $bill->create($bill->state = 1, $bill->total = $totalSale, $bill->saleId = $idInsertSale
                    );
                    $queryBill = $dbConection->ejecutar($billx);
                    //ejecuta la insercion de la factura
                    //print_r($saleDetailx);
                    if (@$queryBill) {
                        $idInsertBill = $dbConection->lastID();
                        //si inserta de forma correcta recupera el id de la insercion
                        $sqlBill = $bill->showOneBillSale($bill->id = $idInsertBill);
                        //obtiene el detalle de todos los articulos comprados por el id de la factura
                        $queryBill2 = $dbConection->mostrar_fila($sqlBill);
                        //echo $sqlBill;
                        foreach ($queryBill2 as $row) {
                            //obtiene toda la informacion de la venta a trav�s del id de la factura
                            $bandera = $row['bil_sal_id'];
                            $b = $bandera;
                            $a = $row['bil_date'];
                            $h = $row['bil_total'];
                            $i = $row['us_name']. " ". $row['us_lastname'];
                            $j = $row['sal_customer'];
                        }
                        @$sqlSaleByBill = $bill->showOneSaleStock($bill->id = $bandera);
                        //obtiene el detalle del producto comprado como nombre, descripcion, id, etc...
                        @$querySaleDetail2 = $dbConection->mostrar_fila($sqlSaleByBill);
                        $bandera = 1;
                        $var = "Factura creada correctamente <br />";
                    } else {
                        $bandera = 2;
                        $var = "Se produjo un error inesperado en la generacion de la factura";
                    }
                } else {
                    $bandera = 2;
                    $var = "Se produjo un error inesperado al realizar la venta<br />";
                }
                $vartotal = $var;
                if ($bandera == 1) {
                    $echox = $message->bill1($message->a = $a, $message->b = $b, $message->c = $idInsertBill, $message->j = $j);
                    //comienza a crear el encabezado de la factura
                    echo $echox;
                    //echo $sqlSaleByBill;
                    if ($querySaleDetail2) {
                        foreach ($querySaleDetail2 as $rowxyz) {
                            //comienza a imprimir el detalle de la factura
                            echo "<tr><td style='text-align:left'>". $rowxyz['sto_id']. "</td><td style='text-align:center' >". $rowxyz['sto_descript']. "</td><td style='text-align:center'>". $rowxyz['sade_quantity']. "</td><td style='text-align:center'>$".number_format($rowxyz['sto_salePrice']). "</td><td style='text-align:right'>$".number_format($rowxyz['sade_subtotal']). "</td></tr>";
                        }
                    } else {
                        echo "Se encontro el siguiente error: ". $sqlSaleByBill;
                    }
                    $echox2 = $message->bill2($message->h = number_format($h), $message->i = $i);
                    //crea el pie de pagina de la factura
                    echo $echox2;
                } else {
                    $echox = $message->mensa($message->mens = $vartotal);
                }
            }
            //faltan datos por registrar
            else
            {
                $vartotal = "Faltan datos por registrar";
            }
            if ($bandera != 1) {
                //echo $message->mensa($message->mens = $vartotal);
                echo $vartotal;
            }




            var temp = JsonConvert.SerializeObject(obj);
            return Json(temp, JsonRequestBehavior.AllowGet);
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
