$(".ajaxProcessSingle").change(function () {
    var datax = {};
    datax.id = $(this).val();
    datax.action = $(this).attr("dataAction");
    datax.owner = $(this).attr("dataOwner");
    if (datax.id != "") {
        var uri = "/" + datax.owner + "/" + datax.action;
        if ($(this).attr("dataBefore") != null) {
            var a = $(this).attr("dataBefore");
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                window[a](response)
            });
        }
        else {
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                //console.log(response);
                bootbox.alert(response);
                //$("#quantityInfo").html(response);
            });
        }
    }
    else {
        bootbox.alert(noData);
    }
})

$(document).on("click", ".ajaxProcessSingleC", function () {
    addToJson();
    var datax = {};
    var response = "no data";
    datax.id = json;
    datax.action = $(this).attr("dataAction");
    datax.owner = $(this).attr("dataOwner");
    
    if (datax.id != "") {
        var uri = "/" + datax.owner + "/" + datax.action;
        if ($(this).attr("databefore") != null) {
            var a = $(this).attr("databefore");
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                window[a](response)
            });
        }
        else {
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                //console.log(response)
                bootbox.alert(response);
            }).fail(function () {
                //console.log(response)
                bootbox.alert(error);
            })
        }
    }
    else {
        bootbox.alert(nodata);
        //console.log(response)
    }
})