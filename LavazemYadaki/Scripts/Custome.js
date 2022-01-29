
$(function () {
    $('input:text:first').focus();
    var $inp = $('input:text');
    $inp.bind('keydown', function (e) {
        var key = e.which;
        if (key == 13) {
            e.preventDefault();
            var nxtIdx = $inp.index(this) + 1;
            $(":input:text:eq(" + nxtIdx + ")").focus();
            $(":input:text:eq(" + nxtIdx + ")").select();
        }
    });

    //$('#hokmTaradodCar').click(function () {
    //    alert('use_mostaer clicked..');

    //});

    $(document).on('click', 'input:checkbox', function (e) {

        var childname = '';

        if ($(this).attr("id") == ('hokmTaradodCar')) {
            childname = 'sh_hokmTaradodMaemoriat';
        }

        if ($(this).attr("id") == ('hokmTaradodMaemoriat')) {
            childname = 'sh_hokmTaradodCar';
        }


        if ($(this).is(':checked')) {
            $('#' + childname).prop('disabled', false);
        }
        else {
            $('#' + childname).prop('disabled', true);
        }



    });

    //$('#use_mostaer').change(function () {

    //    if (!this.checked) {
    //        // It is not checked, show your div...
    //        alert('not checked');
    //    }
    //    else
    //        {
    //        alert('checked');
    //    }

    //});
});


//$("input[type=text]").focusin(function () {
//    $(this).select();
//});

var CallActionOfDropDown = function (ElementTextId, ElementTextName, selectFirst, idForSelect,openMenu) {
    var controller = $('#' + ElementTextName).attr("controller");
    var action = $('#' + ElementTextName).attr("action");
    $.ajax({
        type: "POST",
        url: '/' + controller + '/' + action,
        data: '{Text: "' + $('#' + ElementTextName).val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#dropdown-menu-' + ElementTextName).empty();
            jQuery.each(response.data, function (index, itemData) {
                if (index == 0 && selectFirst) {
                    $('#' + ElementTextName).attr('Value', itemData.Text);
                    $('#' + ElementTextId).attr('Value', itemData.Value);
                    return true;
                }
                if (itemData.Value == idForSelect) {
                    $('#' + ElementTextName).attr('Value', itemData.Text);
                    $('#' + ElementTextId).attr('Value', itemData.Value);
                    $('#input-group-btn-' + ElementTextName).removeClass('open').delay(200);
                    return true;
                }
                $('#dropdown-menu-' + ElementTextName).append('<li><a onclick=ClickElement(\'' + itemData.Value + '\',\'' + itemData.Text.split(' ').join('*') + '\',\'' + ElementTextId + '\',\'' + ElementTextName + '\'); >' + itemData.Text + '</a></li>');
            });
            //var name = 'input-group-btn-' + $(o).attr('name');
            //$('#' + name).addClass('open');
            if (openMenu == true){
                $('#input-group-btn-' + ElementTextName).addClass('open');
            }
        },
        done: function () {
        },
        error: function (response) {
            alert("مشکلی در اجرای برنامه به وجود آمده ");
        }
    });
}
    openMenu = function (ElementTextName) {
        console.log('ElementTextName:' + ElementTextName);
    

        //$('.input-group-btn').removeClass('open');
        //$('#input-group-btn-' + ElementTextName).removeClass('open'); //.addClass('open').delay(1000);
        $('#input-group-btn-' + ElementTextName).addClass('open');
    }
    //_keydown = function (ElementTextId, ElementTextName) {

    //    debugger;
    //    var state = $('#' + ElementTextId).attr('disabled');
    //    if (state == 'disabled')
    //        return false;
    //    $('#' + ElementTextId).attr('Value', '');
    //    //clearTimeout(0);
    //    timeoutId = setTimeout(CallActionOfDropDown(ElementTextId, ElementTextName, false, 0), 10);
    //    clearTimeout(0);
    //    openMenu(ElementTextName);
    //}


    _keydown = function (ElementTextId, ElementTextName,idForSelect,openMenu = true) {

        var state = $('#' + ElementTextId).attr('disabled');
        if (state == 'disabled' && false)
            return false;
        $('#' + ElementTextId).attr('Value', '');
        //clearTimeout(0);

        //timeoutId = setTimeout(, 10);
        CallActionOfDropDown(ElementTextId, ElementTextName, false, idForSelect,openMenu)
        //clearTimeout(0);
        //openMenu(ElementTextName);
    }


    function ClickElement(id, text, ElementTextId, ElementTextName) {
        text = text.replace(/\*/g, ' ');
        $('#' + ElementTextName).val(text);
        $('#' + ElementTextId).val(id);
    }

    click_loadPartial = function (controller, action,idRecord) {
        //debugger;
        //console.log(controller + '-' + action + '-' + idRecord);
        $('#modal_div').empty();
        var url = '/' + controller + '/' + action;
        if (idRecord > 0)
            url += '?id='+ idRecord;
        $.get(url, function (data) {
            $('#modal_div').html(data);
        });
     
    }
