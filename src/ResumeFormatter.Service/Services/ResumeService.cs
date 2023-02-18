﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using ResumeFormatter.Domain.Entities;
using ResumeFormatter.Domain.Interfaces.Service;

namespace ResumeFormatter.Service.Services
{
    public class ResumeService : IResumeService
    {
        private List<Keyword> keyWords = new List<Keyword>()
        {
            new Keyword() { Id = 1, UserId = 1, Word = "Objetivo", CreatedAt = DateTime.Now },
            new Keyword() { Id = 2, UserId = 1, Word = "Habilidades e competência", CreatedAt = DateTime.Now },
            new Keyword() { Id = 3, UserId = 1, Word = "Escolaridade", CreatedAt = DateTime.Now },
            new Keyword() { Id = 4, UserId = 1, Word = "Experiência profissional", CreatedAt = DateTime.Now },
            new Keyword() { Id = 5, UserId = 1, Word = "Conhecimentos", CreatedAt = DateTime.Now },
            new Keyword() { Id = 5, UserId = 1, Word = "Idiomas", CreatedAt = DateTime.Now }
        };

        public byte[] Format(IFormFile template, IFormFile file)
        {
            var templateStream = template.OpenReadStream();
            var fileStream = file.OpenReadStream();

            var fileData = GetFileData(fileStream);

            return this.ProcessDocumentFile(templateStream, fileData);
        }

        private Dictionary<string, dynamic> GetFileData(Stream stream)
        {
            MemoryStream documentStream = new MemoryStream();
            stream.CopyTo(documentStream);

            using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
            {
                template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = template.MainDocumentPart;

                var mainRun = mainPart.Document.Body?.Descendants<Run>();

                foreach (var run in mainRun.Select((value, index) => new { value, index }))
                {
                    foreach (RunProperties props in run.value?.Descendants<RunProperties>())
                    {
                        if (props.Descendants<Bold>().Count() > 0 && props.Descendants<Bold>()?.First() != null && this.keyWords.Any(keyWord => keyWord.Word == run.value.InnerText.Replace(":", "")))
                        {
                            var aa = mainRun.ElementAt(run.index + 1).InnerText;
                        }
                    }
                }

                mainPart.Document.Save();

            }

            documentStream.ToArray();

            return new Dictionary<string, dynamic>();
        }

        private byte[] ProcessDocumentFile(Stream stream, Dictionary<string, dynamic> fileData)
        {
            MemoryStream documentStream = new MemoryStream();
            stream.CopyTo(documentStream);

            using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
            {
                template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = template.MainDocumentPart;

                mainPart.Document.Save();
            }

            return documentStream.ToArray();
        }

        private byte[] EditTextFields(Stream fileStream)
        {
            return null;
        }
    }
}
