/*

    Datagrid utilities.
    04/06/2012
    Martín E. Durán

*/


(function ($) {

    $.fn.updatingDatagrid = function () {
        var bDiv = $(this).find('.blockDiv');
        var lIcon = $(this).find('.bLoading');

        $(lIcon).html('<div class="bGroup"><div class="tButton"><div><span class="loading" style="padding-left:20px">procesando...</span></div></div></div>');
        $(lIcon).show();
        $(bDiv).addClass('gBlock');
        $(bDiv).fadeTo(0, 0.5);
    };

    $.fn.endUpdatingDatagrid = function () {
        var bDiv = $(this).find('.blockDiv');
        var lIcon = $(this).find('.bLoading');

        $(lIcon).html('');
        $(lIcon).hide();
        $(bDiv).removeClass('gBlock');
    };

    $.fn.setRows = function (rows) {
        var rows_container = $(this).find('.scrollable table');

        rows_container.html(rows);
    };

    $.fn.GoFirst = function () {
        var bFirst = $(this).find('.iFirst');

        $(bFirst).click();
    };

    $.fn.bindDatagridEvents = function (updateGridFunction) {
        var bFirst = $(this).find('.iFirst');
        var bPrev = $(this).find('.iPrev');
        var bNext = $(this).find('.iNext');
        var bLast = $(this).find('.iLast');
        var pControl = $(this).find('.dg-page-ctrl');

        $(pControl).attr('current_page', '1').attr('max_pages', '1');

        $(bFirst).unbind('click').click(function () {
            var current_page = _GetCurrentPage();
            var max_pages = _GetMaxPages();

            if (current_page != 1 && max_pages > 1) {
                SetCurrentPage($(pControl), 1, updateGridFunction);
            }
        });
        $(bPrev).unbind('click').click(function () {
            var current_page = _GetCurrentPage();
            var max_pages = _GetMaxPages();

            if (current_page > 1) {
                SetCurrentPage($(pControl), parseInt(current_page) - 1, updateGridFunction);
            }
        });
        $(bNext).unbind('click').click(function () {
            var current_page = _GetCurrentPage();
            var max_pages = _GetMaxPages();

            if (current_page < max_pages) {
                SetCurrentPage($(pControl), parseInt(current_page) + 1, updateGridFunction);
            }
        });
        $(bLast).unbind('click').click(function () {
            var current_page = _GetCurrentPage();
            var max_pages = _GetMaxPages();

            if (current_page != max_pages && max_pages > 1) {
                SetCurrentPage($(pControl), max_pages, updateGridFunction);
            }
        });
        $(pControl).unbind('keypress').keypress(function (e) {
            var current_page = $(this).val();
            var max_pages = _GetMaxPages();

            if (!IsNumeric(current_page)) {
                current_page = 1;
            }
            else if ($(this).val() > max_pages) {
                current_page = max_pages;
            }

            if (e.which == 13) {
                SetCurrentPage($(this), current_page, updateGridFunction);
            }
        });

        function _GetCurrentPage() {
            return parseInt($(pControl).attr('current_page'));
        }
        function _GetMaxPages() {
            return parseInt($(pControl).attr('max_pages'));
        }
    };

    function SetCurrentPage(ctrl, c, updateGridFunction) {
        $(ctrl).attr('current_page', c).val(c);

        updateGridFunction();
    }

    $.fn.SetMaxPages = function (m) {
        var pControl = $(this).find('.dg-page-ctrl');
        var pCount = $(this).find('.dg-page-count');

        $(pControl).attr('max_pages', m);
        $(pCount).text(m);

        if (m == 1) {
            $(pControl).attr('current_page', 1).val(1);
        }
    };

    $.fn.GetCurrentPage = function () {
        var pControl = $(this).find('.dg-page-ctrl');

        return $(pControl).attr('current_page');
    };
})(jQuery);