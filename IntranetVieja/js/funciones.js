var dayNames = new Array('Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado');
var dayNamesMin = new Array('Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá');
var monthNames = new Array('Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre');

var MsgOperationError = 'Se produjo un error al intentar completar la operación.';

/*
 * Fixes para IE.
 */
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (obj) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == obj) {
                return i;
            }
        }
        return -1;
    }
}

/*
    Cadena: string a completar.
    Caracter: caracter con el que se va a completar la cadena.
    Largo: largo final de la cadena.
    Delante: determina si los caracteres se completan al inicio o al final.
*/
function CompletarCon(cadena, caracter, largo, delante) {
    var result = delante ? '' : cadena;
    var i;

    for (i = 0; i < (largo - cadena.length); i++) {
        result += caracter.toString();
    }

    result += delante ? cadena : '';

    return result;
}

function ArmarFecha(strFecha) {
    var a = strFecha.split('/');

    result = new Date();
    result.setDate(a[0]);
    result.setMonth(a[1] - 1);
    result.setYear(a[2]);

    return result;
}

function FechaToString(fecha) {
    return CompletarCon(fecha.getDate().toString(), '0', 2, true) + '/'
         + CompletarCon((fecha.getMonth() + 1).toString(), '0', 2, true) + '/'
         + CompletarCon(fecha.getFullYear().toString(), '0', 2, true);
}

function DateTimeToString(datetime) {
    // Las fechas en ASP.NET son devueltas en un formato /Date(1289358000000)/.

    var result;

    aux = datetime.toString().replace('/Date(', '');
    aux = aux.replace(')/', '');

    var ms = parseInt(aux, 0);

    var date = new Date(ms);

    if (isNaN(date)) {
        result = '';
    }
    else {
        result = FechaToString(date);
    }

    return result;
}

function MaxLength(obj) {
    var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : ""
    if (obj.getAttribute && obj.value.length > mlength)
        obj.value = obj.value.substring(0, mlength)
}

function CadenaVacia(cadena) {
    var result;
    var aux;

    aux = cadena.replace(/^(\s|\&nbsp;)*|(\s|\&nbsp;)*$/g, "");
    result = aux.length == 0;

    return result;
}

// Reemplaza los caracteres inválidos para jQuery.
function RInv(s) {
    if (s == null) return null;

    return s.replace('"', "\\" + '"');
};

function AcortarTexto(texto, max) {
    var result = '';

    if (max >= texto.length) {
        result = texto;
    }
    else {
        for (var i = 0; i < max; i++) {
            result += texto[i];
        }
        result += '...';
    }

    return result;
}

function RemoveArrayElement(array, element) {
    if (array) {
        var idx = array.indexOf(element);
        if (idx != -1) {
            array.splice(idx, 1);
        }
    }
}

var ConsultaAjax = {
    url: '',
    data: null,
    cache: false,
    async: false,
    AjaxSuccess: function (msg) {
    },
    AjaxError: function (msg) {
    },
    Restablecer: function () {
        // Restablezco los valores.
        this.url = '';
        this.data = null;
        this.cache = false;
        this.async = false;
        this.AjaxSuccess = null;
        this.AjaxError = null;
    },
    Ejecutar: function () {
        $.ajax({
            url: this.url,
            type: "POST",
            dataType: "json",
            data: this.data,
            contentType: "application/json; charset=utf-8",
            async: this.async,
            cache: this.cache,
            success: function (msg) {
                if (ConsultaAjax.AjaxSuccess != null) {
                    ConsultaAjax.AjaxSuccess(msg);
                }

                ConsultaAjax.Restablecer();
            },
            error: function (data, ajaxOptions, thrownError) {
                errno = eval("(" + data.responseText + ")").Message;
                if (ConsultaAjax.AjaxError != null) {
                    ConsultaAjax.AjaxError(errno);
                }

                ConsultaAjax.Restablecer();
            }
        });
    }
}

var custom_dialog = {
    primary_button_click: '',
    show: function (type, message) {
        if ($('#main_messagebox').html() == '') {
            $('#main_messagebox').addClass('messagebox');
            $('#main_messagebox').html('<div class="header"><span></span></div></div><div class="footer"><ul class="button_list"></ul></div>');
        }

        $('#main_messagebox').addClass(type);

        $('#main_messagebox .header').find('span').html(message);

        $.modal.close();
        $('#main_messagebox').modal({ opacity: 50, overlayCss: { backgroundColor: "#000" }, escClose: false });
    },
    close: function () {
        $.modal.close();
        $('#main_messagebox').removeClass('success').removeClass('info').removeClass('warning').removeClass('error');
        $('#main_messagebox .footer .button_list').html('');
    },
    buttons: function (showButtons, showSecondaryAction, textPrimaryAction, textSecondaryAction, primaryAction, secondaryAction) {
        if (showButtons) {
            $('#main_messagebox .footer .button_list').html('<li id="mbPrimaryAction" onclick="' + primaryAction + '"><div class="btn primary_action_button_small button_100"><div class="cap"><span>' + textPrimaryAction + '</span></div></div></li>');

            if (showSecondaryAction) {
                $('#main_messagebox .button_list').append('<li id="mbSecondaryAction" onclick="' + secondaryAction + '"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>' + textSecondaryAction + '</span></div></div></li>');
            }

            $('#main_messagebox .footer').show();
        }
        else {
            $('#main_messagebox .footer .button_list').html('');
            $('#main_messagebox .footer').hide();
        }
    }
}

String.prototype.startsWith = function (str) {
    return (this.match("^" + str) == str)
}

function DateDiff(date1, date2) {
    var d1 = Date.parse(date1);
    var d2 = Date.parse(date2);

    var ms_day = 1000 * 60 * 60 * 24;
    var days = (d2 - d1) / ms_day;

    return days;
}

function GetAjaxError(data) {
    var errno = eval("(" + data.responseText + ")").Message;

    return errno;
}

function GetNumber(s) {
    var result = s;

    if (s.replace) {
        result = s.replace(',', '.');
    }

    return result;
}

function ShortenText(text, max) {
    var result = '';
    var more = '...';
    var i;

    if (text.length < max) {
        max = text.length;
        more = '';
    }
    else {
        max -= 3; // Le resto 3 caracteres que son los ... del final.
    }

    for (i = 0; i < max; i++) {
        result += text.charAt(i);
    }
    result += more;

    return result;
}