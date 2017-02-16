var inventory = "@ViewBag.Message";
var totalBuy = [];
var dataOfSale = {};
var auto = 0;
var noData = "<div class='alert alert-danger'><h3 style='text-align:center'>Faltan datos por registrar</h3></div>";

$.getScript("/Scripts/bootbox.min.js");

/**
*función que se encarga de agregar la cantidad de productos disponibles en el desplegable
*/
function quantityOption(x) {
    var json = JSON.parse(x);
    var quantity = $("#DropDownListQuantity");
    if (json.state == "ok") {
        quantity.empty(); // remove any existing options
        for (var i = 1; i <= parseInt(json.answer) ; i++) {
            quantity.append($('<option>', {
                value: i,
                text: i
            }));
        }
    }
}

/**
*función que se encarga de agregar el valor del producto seleccionado en el campo unitario
*/
$(".stockId").on("change", function () {
    var id = $(this).val();
    var price = $(".stockPrice option[value='" + id + "']").text()
    $("#unitary").val(price);
})

/**
*función que se encarga de poner el subtotal en el campo subtotal
*/
$("#DropDownListQuantity").on("change", function () {
    var howMany = $(this).val();
    var price = ($("#unitary").val() * howMany);
    $("#subtotal").val(price);
});

/**
*función que se encarga de agregar los productos al carrito de compras
*/
$(".addCar").on("click", function () {
    //se obtinene los campos seleccionados
    var detail = {};
    var productId = $(".stockId").val();
    var productDescription = $(".stockId option:selected").text();
    var productQuantity = $("#DropDownListQuantity").val();
    var productUnitary = $("#unitary").val();
    var productSubtotal = $("#subtotal").val();
    //se agrega la imagen para eliminar el producto de la lista
    var removeRow = "<img src='/assets/images/cancell.png' class='preimg removeRow' />"
    //si todos los campos están seleccionados
    if (productUnitary != '' && productSubtotal != '') {
        //eliminamos los colores de los campos resaltados
        var fullTextBoxes = $('input:text').filter(function () { return this.value != ""; });
        fullTextBoxes.each(function () {
            $(this).removeClass("bg-danger");
        });
        detail.productId = productId;
        detail.productDescription = productDescription;
        detail.productQuantity = productQuantity;
        detail.productSubtotal = productSubtotal;
        //agregamos los datos del producto al array maestro
        totalBuy.push(
            detail
            );
        //agregamos los datos a la tabla detalle
        $("#billif").find('tbody').append($('<tr><td class="">' + auto + '</td><td class="">' + productId + '</td><td>' + productDescription + '</td><td>' + productQuantity + '</td><td>' + productUnitary + '</td><td>' + productSubtotal + '</td><td>' + removeRow + '</td></tr>'));
        auto++;
        //limpiamos los campos
        $("#unitary,#subtotal").val("");
        //deshabilitamos el producto del desplegable
        $(".stockId option[value='" + productId + "']").prop('disabled', true);
        $("#totalSale").html();
    }
    else {
        //agregamos el resaltado en caso de datos faltantes
        var emptyTextBoxes = $('input:text').filter(function () { return this.value == ""; });
        emptyTextBoxes.each(function () {
            $(this).addClass("bg-danger");
        });
    }

});

function arrSum() {
    for (var x = 0; x < totalBuy.length; x++) {
        for (var i = 0; i < totalBuy[x].length; i++) {
            console.log(totalBuy[x][i]);
        }
    }
}

$(document).on("click", ".removeRow", function () {
    //elegimos la posición del array a eliminar
    var idRemove = $(this).parent().parent().children("td:eq(0)").html();
    //eliminamos la posición
    totalBuy.splice(idRemove, 1);
    //quitamos la fila de la tabla detalle
    $(this).parent().parent().fadeOut();
    //obtenemos el id del producto a habilitar en el desplegable
    idRemove = $(this).parent().parent().children("td:eq(1)").html();
    //habilitamos la opción del desplegable
    $(".stockId option[value='" + idRemove + "']").removeProp('disabled');
});

/**
*función que se encarga de finalizar la compra tomando los datos del vendedor, comprador y el detalle de la venta.
*/
$(document).on("click", ".billFinal", function () {
    if ($.trim($("[name=cliId]").val()) != '') {
        $("[name=cliId]").removeClass("bg-danger");
        //obtenemos los datos del vendedor
        dataOfSale.salePerson = 1010;
        //obtenemos los datos del cliente
        dataOfSale.clientPerson = $("[name=cliId]").val();
        //obtenemos el detalle de la venta escogida
        dataOfSale.detail = JSON.stringify(totalBuy);
        //convertimos en json
        var json = JSON.stringify(dataOfSale);
        //agregamos el json al atributo id para realizar el proceso ajax
        $(this).attr("id", json);
    }
    else {
        $("[name=cliId]").addClass("bg-danger");
    }

})

$(document).on("click", ".viewArray", function () {
    console.log(totalBuy);
    arrSum();
});