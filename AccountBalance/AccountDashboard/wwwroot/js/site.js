$(function () {
    "use strict";

    // ------------------------------------------------------------------
    // Generic AJAX form submit for the Purchase / Payment "create" forms.
    // Falls back to a normal browser POST automatically if JS is off,
    // because the form's plain action/method attributes still work.
    // ------------------------------------------------------------------
    $(".ajax-form").on("submit", function (e) {
        e.preventDefault();

        var $form = $(this);
        var $submitBtn = $form.find("button[type=submit]");
        var $feedback = $form.find(".form-feedback");

        $feedback.empty();
        $submitBtn.prop("disabled", true).data("original-text", $submitBtn.text());

        $.ajax({
            url: $form.attr("action"),
            method: "POST",
            data: $form.serialize(),
            headers: { "X-Requested-With": "XMLHttpRequest" }
        })
        .done(function (response) {
            $feedback.html(
                '<div class="alert alert-success" role="status">' +
                escapeHtml(response.message || "Operación registrada correctamente.") +
                "</div>"
            );
            $form.trigger("reset");

            var redirectUrl = $form.data("redirect-url");
            if (redirectUrl) {
                setTimeout(function () { window.location.href = redirectUrl; }, 900);
            }
        })
        .fail(function (xhr) {
            var message = "No se pudo completar la operación. Intenta nuevamente.";
            var details = [];

            if (xhr.responseJSON) {
                if (xhr.responseJSON.error) message = xhr.responseJSON.error;
                if (Array.isArray(xhr.responseJSON.details)) details = xhr.responseJSON.details;
            }

            var html = '<div class="alert alert-error" role="alert">' + escapeHtml(message);
            if (details.length) {
                html += "<ul>" + details.map(function (d) { return "<li>" + escapeHtml(d) + "</li>"; }).join("") + "</ul>";
            }
            html += "</div>";

            $feedback.html(html);
        })
        .always(function () {
            $submitBtn.prop("disabled", false).text($submitBtn.data("original-text"));
        });
    });

    // ------------------------------------------------------------------
    // "Exportar en PDF": no backend PDF endpoint exists yet, so this opens
    // the browser's native print dialog against the print-optimized view
    // (see AccountStatementController.Print + Views/AccountStatement/Print.cshtml).
    // ------------------------------------------------------------------
    $(".js-print-statement").on("click", function () {
        window.print();
    });

    function escapeHtml(str) {
        return $("<div>").text(str).html();
    }
});
