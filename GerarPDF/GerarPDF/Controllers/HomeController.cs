using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GerarPDF.Models;
using PdfSharpCore.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace GerarPDF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHostingEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Método Criado para o PDF
        public FileResult GerarRelatorio()
        {

            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphics = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var corFonte = PdfSharpCore.Drawing.XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var fonteOrganzacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                var fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                var titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                var fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 7);
                var webRoot = _environment.WebRootPath;
                var logo = string.Concat(webRoot, "/imagens/logo.jpg");
                var qtdPaginas = doc.PageCount;
                textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));

                // Impressão do LogoTipo
                XImage imagem = XImage.FromFile(logo);
                graphics.DrawImage(imagem, 20, 5, 300, 50);


                // Titulo Exibição
                textFormatter.DrawString("Nome :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 75, page.Width, page.Height));
                textFormatter.DrawString("Valdir Ferreira ", fonteOrganzacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 75, page.Width, page.Height));

                textFormatter.DrawString("Profissão :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 95, page.Width, page.Height));
                textFormatter.DrawString("Programador", fonteOrganzacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 95, page.Width, page.Height));

                textFormatter.DrawString("Tempo :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 115, page.Width, page.Height));
                textFormatter.DrawString("10 anos", fonteOrganzacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 115, page.Width, page.Height));


                // Titulo maior 
                var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                tituloDetalhes.DrawString("Detalhes ", titulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 120, page.Width, page.Height));


                // titulo das colunas
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Descrição", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Atendimento", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(144, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Operação", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(220, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Quantidade", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Status", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(337, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Data", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(400, alturaTituloDetalhesY, page.Width, page.Height));


                //dados do relatório 
                var alturaDetalhesItens = 160;
                for (int i = 1; i < 30; i++)
                {

                    textFormatter.DrawString("Descrição" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(21, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Atendimento" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(145, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Operação" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(215, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Quantidade" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString("Status" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(332, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(DateTime.Now.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(400, alturaDetalhesItens, page.Width, page.Height));

                    alturaDetalhesItens += 20;
                }



                #region //ADICIONAR NOVA PAGINA 

                page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                graphics = XGraphics.FromPdfPage(page);
                corFonte = XBrushes.Black;

                fonteOrganzacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 7);
                detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);



                logo = string.Concat(webRoot, "/imagens/logo.jpg");
                qtdPaginas = doc.PageCount;
                detalhes.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));

                // Impressão do LogoTipo
                imagem = XImage.FromFile(logo);
                graphics.DrawImage(imagem, 20, 5, 300, 50);

                var alturaDetalhesItensPageNew = 160;
                for (int i = 1; i < 30; i++)
                {

                    detalhes.DrawString("Descrição" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(21, alturaDetalhesItensPageNew, page.Width, page.Height));
                    detalhes.DrawString("Atendimento" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(145, alturaDetalhesItensPageNew, page.Width, page.Height));
                    detalhes.DrawString("Operação" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(215, alturaDetalhesItensPageNew, page.Width, page.Height));
                    detalhes.DrawString("Quantidade" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaDetalhesItensPageNew, page.Width, page.Height));
                    detalhes.DrawString("Status" + " : " + i.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(332, alturaDetalhesItensPageNew, page.Width, page.Height));
                    detalhes.DrawString(DateTime.Now.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(400, alturaDetalhesItensPageNew, page.Width, page.Height));

                    alturaDetalhesItensPageNew += 20;
                }
                #endregion


                using (MemoryStream stream = new MemoryStream())
                {
                    var contantType = "application/pdf";
                    doc.Save(stream, false);

                    var nomeArquivo = "RelPDF.pdf";

                    //return File(stream.ToArray(), contantType, nomeArquivo);

                    FileContentResult result = new FileContentResult(stream.ToArray(), "application/pdf");


                    return result;
                }



            }

            //return View();
        }
    }
}
