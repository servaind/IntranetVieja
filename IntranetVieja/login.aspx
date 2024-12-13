<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Servaind ::: Intranet</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="/js/jquery.js" type="text/javascript"></script>
    <script language="javascript" src="/js/jquery.simplemodal.js" type="text/javascript"></script>
    <script language="javascript" src="/js/fx.shadow.js" type="text/javascript"></script>
    <script language="javascript" src="/js/jquery.corners.js" type="text/javascript"></script>
    <script language="javascript" src="/js/funciones.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('.messagebox').corner();

            $('.input_wrapper input').focus(function () {
                $(this).parents('.input_wrapper').addClass('input_wrapper_active');
            }).blur(function () {
                $(this).parents('.input_wrapper').removeClass('input_wrapper_active');
            });
            $('.textarea_wrapper textarea').focus(function () {
                $(this).parents('.textarea_wrapper').addClass('textarea_wrapper_active');
            }).blur(function () {
                $(this).parents('.textarea_wrapper').removeClass('textarea_wrapper_active');
            });

            $('.messagebox .close').click(function () {
                CerrarVentana();
            });

            $('select').change(function () {
                var current_value = $(this).val();
                if (current_value != null) {
                    var current_text = $('#' + $(this).attr('id') + ' option[value=' + current_value + ']').text();
                    $(this).prev('span').text(current_text);
                }
            });

            $('#txtUsuario').keydown(function (event) {
                if (event.keyCode == 13) {
                    $('#btnAceptar').click();
                }
            });

            $('#txtPassword').keydown(function (event) {
                if (event.keyCode == 13) {
                    $('#btnAceptar').click();
                }
            });

            $('#btnAceptar').click(function () {
                var usr = $('#txtUsuario');
                var pwd = $('#txtPassword');
                var result = true;

                if (jQuery.trim(usr.val()).length == 0) {
                    usr.parents('.input_wrapper').addClass('input_wrapper_error');
                    result = false;
                }
                else {
                    usr.parents('.input_wrapper').removeClass('input_wrapper_error');
                }

                if (jQuery.trim(pwd.val()).length == 0) {
                    pwd.parents('.input_wrapper').addClass('input_wrapper_error');
                    result = false;
                }
                else {
                    pwd.parents('.input_wrapper').removeClass('input_wrapper_error');
                }

                if (result) {
                    MostrarVentana('loading');

                    ConsultaAjax.url = 'login.aspx/Login';
                    ConsultaAjax.data = '{ "usr":"' + encodeURIComponent(usr.val()) + '", "pwd":"' + encodeURIComponent(pwd.val()) + '", "chk": "' +
                                        +($('#chkRecordar').is(':checked') ? '1' : '0') + '" }';
                    ConsultaAjax.AjaxSuccess = function (msg) {
                        location.href = msg.d;
                    };
                    ConsultaAjax.AjaxError = function (msg) {
                        ErrorMsg(msg);
                    };

                    ConsultaAjax.Ejecutar();
                }
            });

            $('#btnCancelar').click(function () {
                MostrarVentana('loading');

                location.href = 'http://www.servaind.com.ar';
            });

            $('#txtPassword').val('<%= this.password %>');

            $('#login').shadow({ offset: 5, opacity: 0.15 });
            $('#txtUsuario').focus();
        });

        function Mensaje(message, mClass, showButtons, showSecondaryAction, textPrimaryAction, textSecondaryAction, primaryAction, secondaryAction) {
            custom_dialog.show(mClass, message);
            custom_dialog.buttons(showButtons, showSecondaryAction, textPrimaryAction, textSecondaryAction, primaryAction, secondaryAction);
        }

        function MostrarVentana(div) {
            $.modal.close();

            $('#' + div).modal({ opacity: 50, overlayCss: { backgroundColor: "#000" }, persist: true, escClose: false });
        }

        function CerrarVentana() {
            $.modal.close();
        }

        var ult_ventana = null;
        function ErrorMsg(mensaje) {
            Mensaje(mensaje, 'error', true, false, 'Aceptar', '', 'OnErrorAceptar()', '');
        }
        function OnErrorAceptar() {
            if (ult_ventana != null) {
                MostrarVentana(ult_ventana);
                ult_ventana = null;
            }
            else {
                $.modal.close();
            }
        }
    </script>
</head>
<body>
    <div class="dialog_wrapper" id="login">
        <div class="dialog_header">
            <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Intranet Servaind - Iniciar sesión</h3></div></div></div>
        </div>

        <div class="dialog_content">
            <ul class="middle_form">
                <li class="form_floated_item normal">
                    <label class="label" for="txtUsuario">Usuario:</label>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtUsuario" runat="server" type="text"/>
                        </div>
                    </div>
                </li>
                <li class="form_floated_item normal">
                    <label class="label" for="txtPassword">Contraseña</label>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtPassword" runat="server" value="asdsa" type="password"/>
                        </div>
                    </div>
                </li>
                <li class="form_floated_item normal">
                    <input id="chkRecordar" runat="server" type="checkbox" runat="server" checked="checked"/><label class="checkboxlabel" for="chkRecordar">Recordar</label>
                </li>
            </ul>
        </div>

        <div class="dialog_footer">
            <ul class="button_list">
                <li><div class="btn primary_action_button_small button_100" id="btnAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
                <li><div class="btn secondary_action_button_small button_100" id="btnCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
            </ul>
        </div>
    </div>

    <div id="main_messagebox"></div>

    <div id="loading">

    </div>
</body>
</html>
