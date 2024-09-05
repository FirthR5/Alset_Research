var pdfjsLib = window['pdfjs-dist/build/pdf'];
pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.6.347/pdf.worker.min.js';
var scale = 1;
var resolution = 1;

function LoadPdfFromUrl(url, containerId) {
    try {
        var pdf_container = document.getElementById("pdf_container_" + containerId);
        pdf_container.innerHTML = "";

        pdfjsLib.getDocument(url).promise.then(function (pdfDoc) {
            for (var i = 1; i <= pdfDoc.numPages; i++) {
                RenderPage(pdfDoc, pdf_container, i);
            }
        });
    }
    catch (error) {
        console.error("Error:", error);
    }
}

function RenderPage(pdfDoc, pdf_container, num) {
    pdfDoc.getPage(num).then(function (page) {
        var canvas = document.createElement('canvas');
        var ctx = canvas.getContext('2d');
        pdf_container.appendChild(canvas);

        var viewport = page.getViewport({ scale: scale });
        canvas.height = resolution * viewport.height;
        canvas.width = resolution * viewport.width;

        var renderContext = {
            canvasContext: ctx,
            viewport: viewport,
            transform: [resolution, 0, 0, resolution, 0, 0]
        };

        page.render(renderContext);
    });
}