using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.Common.Details;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.UseCases.Commmands.AddUserCommand;
using MajlesMefa.Back.UseCases.Commmands.DeleteUseCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateUserCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetSenatorProfileQuery;
using MajlesMefa.Back.UseCases.Queries.GetUserByIdQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;
using System.Data;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using IOFile = System.IO.File;
using MajlesMefa.Back.Utilities.OpenXML;


namespace MajlesMefa.UI.Views.Senator
{
    public class SenatorController : BaseController
    {

        public const string CONTROLLER = "Senator";
        private readonly string _baseUrl;
        private readonly string _baseFileLocation;
        private readonly ILogger<SenatorController> _logger;

        public SenatorController(IMapper mapper, ILogger<SenatorController> logger, IConfiguration configuration) : base(mapper)
        {
            _logger = logger;
            _baseUrl = configuration.GetSection("FileServer:BaseFileUrl").Value;
            _baseFileLocation = configuration.GetSection("FileServer:BaseFileLocation").Value;
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Display(Name = "مدیریت نمایندگان")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Index(Guid? commissionId)
        {
            Guid commissionIdValue = commissionId.HasValue ? commissionId.Value : @Guid.Empty;
            var commissiontitle = commissionIdValue == Guid.Empty ? null : await getCommissionTitleById(commissionIdValue);
            ViewBag.CommissionTitle = commissiontitle;

            return View(commissionId);
        }


        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpGet]
        public IActionResult PrintChoice(Guid senatorId)
        {
            var printData = new PrintChiocesVm()
            {
                Id = senatorId,
            };
            return View(printData);
        }


        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Report(Guid senatorId)
        {
            var query = new GetSenatorProfileQuery
            {
                UserId = senatorId,
            };
            var list = await Mediator.Send(query);
            var userQuery = new GetUserByIdQuery()
            {
                UserId = senatorId
            };
            var udate = await Mediator.Send(userQuery);
            var rslt = list.Items.SingleOrDefault();
            rslt.Email = udate.Email;
            SenatorVm senatorVm = new SenatorVm()
            {
                Id = senatorId,
                ProfilePhotoUrl = rslt.ProfilePhoto is not null ? _baseUrl + rslt.ProfilePhoto : null,
                HozeCityName = rslt.HozeCity.Name,
                CityName = rslt.City.Name,
                Name = rslt.Name,
                Mobile = rslt.Mobile,
                ComissionMembershipTitle = await getCommissionTitleById(rslt.ComissionMembership),
                PersonalFavorites = rslt.PersonalFavorites,
                HozeCityId = rslt.HozeCity.Id,
                BirthCityParentId = rslt.BirthCity.ParentId,
                PoliticalTending = rslt.PoliticalTending,
                CityId = rslt.City.Id,
                Email = rslt.Email,
                FractionMembership = rslt.FractionMembership,
                SocialActivity = rslt.SocailActivity,
                SenaHistory = rslt.SenaHistory,
                JobHistory = rslt.JobHistory,
                SabegheHeyatReise = rslt.SabegheHeyatReise,
                SabegheEmzaEstizah = rslt.SabegheEmzaEstizah,
                Reshte = rslt.Reshte,
                BirthdateString = rslt.BirthDate.ToPersianDate(),
                Birthdate = rslt.BirthDate,
                HozeEntekhabiInt = (int)rslt.HozeEntekhabi,
                ShoghleGhalebInt = (int)rslt.ShoghleGhaleb,
                MadrakTahsiliInt = (int)rslt.MadrakTahsili,
                MahaleTahsilInt = (int)rslt.MahaleTahsil,
                GerayeshSiasiInt = (int)rslt.GerayeshSiasi,
            };

            //var layeheQuery = new GetDataEntriesQuery()
            //{
            //    SenatorIdId = senatorId,
            //    DataEntryType = DataEntryTypeEnum.Layehe,
            //};
            //var layeheList = await Mediator.Send(layeheQuery);
            //var layeheRslt = DataSourceResult.GetFromTable(layeheList);


            SenatorReportVm senatorReportVm = new SenatorReportVm()
            {
                InitialData = senatorVm,
                //Layehe = layeheRslt
            };

            return View(senatorReportVm);
        }


        public async Task<TableModel<DataEntryDto>> GetDataEntries(Guid senatorId, DataEntryTypeEnum dataEntryType)
        {
            var query = new GetDataEntriesQuery()
            {
                SenatorIdId = senatorId,
                DataEntryType = dataEntryType,
                Filter = new TableRequestModel()
                {
                    Skip = 0,
                    Take = -1
                }
            };
            var list = await Mediator.Send(query);
            return list;
        }

        public async Task<string> getCommissionTitleById(Guid id)
        {
            var commissionQuery = new GetDataEntryDetailQuery
            {
                DataEntryId = id,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            };
            var commissionRslt = await Mediator.Send(commissionQuery);
            return commissionRslt.Title;
        }


        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> DownloadWord(PrintChiocesVm printChioces)
        {
            var senatorVm = await GetSenatorById(printChioces.Id);
            string fileName = $"{senatorVm.Name}_{DateTime.Now:yyyyMMdd}.docx";
            MemoryStream ms = new MemoryStream();
            using (WordprocessingDocument doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body body = mainPart.Document.AppendChild(new Body());



                ParagraphProperties paragraphProperties = new ParagraphProperties(
                        new ParagraphStyleId() { Val = "Normal" },
                        new BiDi() { Val = true } // This sets RTL direction
                    );

                // Create a style definition for the default style with RTL direction
                StyleDefinitionsPart stylePart = mainPart.AddNewPart<StyleDefinitionsPart>();
                Styles styles = new Styles();
                Style style = new Style()
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "Normal",
                    Default = true,
                    StyleParagraphProperties = new StyleParagraphProperties(
                        new BiDi() { Val = true } // RTL for default style
                    )
                };
                styles.Append(style);
                stylePart.Styles = styles;




                // Set RTL direction
                SectionProperties sectionProps = new SectionProperties(
                    new BiDi() { Val = true },
                    new PageSize() { Orient = PageOrientationValues.Landscape },
                    new PageBorders()
                    {
                        Display = PageBorderDisplayValues.AllPages,  // Apply to all pages
                        //OffsetFrom = PageBorderOffsetFromValues.Page, // Border outside the text area
                        ZOrder = PageBorderZOrderValues.Front // Keep border visible in front
                    }
                );
                // Define borders for all four sides
                sectionProps.Append(new PageBorders(
                    new TopBorder() { Val = BorderValues.Single, Size = 15, Space = 30, Color = "000000" },  // Top border
                    new BottomBorder() { Val = BorderValues.Single, Size = 15, Space = 30, Color = "000000" },  // Bottom border
                    new LeftBorder() { Val = BorderValues.Single, Size = 15, Space = 30, Color = "000000" },  // Left border
                    new RightBorder() { Val = BorderValues.Single, Size = 15, Space = 30, Color = "000000" }  // Right border
                ));
                body.Append(sectionProps);
                mainPart.Document.Body.Append(new Paragraph(new Run())); // بستن سکشن قبلی
                mainPart.Document.Body.Append(new SectionProperties(
                    new PageSize() { Orient = PageOrientationValues.Landscape, Width = 16840, Height = 11900 }, // A4 landscape size in twips
                    new PageMargin() { Top = 1440, Right = 1440, Bottom = 1440, Left = 1440 }, // optional: standard margins
                    new BiDi() { Val = true }
                ));


                // Create header table
                Table headerTable = new Table();
                headerTable.AppendChild(new TableProperties(
                    new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                    //new TableDirection() { Val = TableDirectionValues.RightToLeft } // Set RTL
                ));

                TableRow bismillahRow = new TableRow(
                    new TableRowProperties(
                            new Justification() { Val = JustificationValues.Center }
                        )
                    );
                // Add Bismillah cell
                TableCell bismillahCell = new TableCell(new TableCellProperties(
                    new GridSpan() { Val = 2 }, // Merge two columns
                    new TableCellWidth() { Width = "100%", Type = TableWidthUnitValues.Pct }
                ));

                Paragraph bismillahParagraph = new Paragraph(
                    new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center } // Center align
                ),
                    new Run(new Text("بسمه تعالی"))
                );
                bismillahCell.Append(bismillahParagraph);
                bismillahRow.Append(bismillahCell);
                headerTable.Append(bismillahRow);

                // Row 2: Photo (Left) and Info (Right)
                TableRow infoRow = new TableRow();

                int imageHeight = 100;
                int twipsPerPixel = 15;
                int imageHeightInTwips = imageHeight * twipsPerPixel;

                TableCell photoCell = new TableCell(new TableCellProperties(
                    new TableCellWidth() { Width = "20%", Type = TableWidthUnitValues.Pct }
                ));

                if (!string.IsNullOrEmpty(senatorVm.ProfilePhotoStr))
                {

                    string profilePhotoPath = Path.Combine(_baseFileLocation, senatorVm.ProfilePhotoStr);

                    if (IOFile.Exists(profilePhotoPath))
                    {
                        // The file exists; proceed to add it to the document
                        ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                        using (FileStream fs = new FileStream(profilePhotoPath, FileMode.Open, FileAccess.Read))
                        {
                            imagePart.FeedData(fs);
                        }
                        ExportTools.AddImageToCell(photoCell, mainPart.GetIdOfPart(imagePart), 100, 120);
                        infoRow.Append(photoCell);
                    }
                }
                else
                {
                    photoCell = new TableCell(new Paragraph(new Run(new Text(""))));
                    infoRow.Append(photoCell);
                }

                // Right cell - Teacher info
                TableCell rightCell = new TableCell(new TableCellProperties(
                    new TableCellWidth() { Width = "80%", Type = TableWidthUnitValues.Pct }
                ));
                var meetingTxt = string.Empty;
                var molaghatList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.Molaghat);
                var dates = molaghatList.Items
                        .Where(x => ((MolaghatDto)x.MyData).IsMolaghatBaVazir)
                        .Select(x => ((MolaghatDto)x.MyData).Tarikh.ToPersianDate())
                        .ToList();
                if (dates.Count > 0)
                {
                    

                    //meetingTxt = $"\u200F)) {dates.Count} بار : {string.Join(" - ", dates)} ((\u200F";
                    meetingTxt = $" {dates.Count} بار مورخ {string.Join(" - ", dates)}";
                    //meetingTxt = $"{string.Join(" - ", dates)} : بار {dates.Count}  {string.Join(" - ", dates)} ";
                    //meetingTxt = $"{string.Join(" - ", dates)} : بار {dates.Count}  {string.Join(" - ", dates)} ";
                }
                else
                {
                    meetingTxt = " \u200F-\u200F ";
                }
                rightCell.Append(ExportTools.CreateBoldNameParagraph("نام و نام خانوادگی : ‌", senatorVm.Name));
                rightCell.Append(ExportTools.CreateBoldNameParagraph("استان :‌‌ ", senatorVm.CityName));
                rightCell.Append(ExportTools.CreateBoldNameParagraph("حوزه انتخابیه  :‌ ‌", senatorVm.HozeCityName));
                rightCell.Append(ExportTools.CreateBoldNameParagraph($"کمیسیون : ‌", senatorVm.ComissionMembershipTitle));
                rightCell.Append(ExportTools.CreateBoldNameParagraph($"‌سابقه نمایندگی : ", senatorVm.SenaHistory));
                rightCell.Append(ExportTools.CreateBoldMeetingParagraph("تعداد ملاقات : ‌", meetingTxt));
                rightCell.Append(ExportTools.CreateBoldNameParagraph("سوابق قبلی : ", senatorVm.JobHistory));

                infoRow.Append(rightCell);
                headerTable.Append(infoRow);
                body.Append(headerTable);

                Paragraph horizontalLineParagraph = new Paragraph(
                    new ParagraphProperties(
                    new ParagraphBorders(
                    new BottomBorder
                    {
                        Val = BorderValues.Single,
                        Color = "000000", // Black color
                        Size = 12, // Border size (1/8 pt, so 12 is 1.5 pt)
                        Space = 1 // Space between text and border
                    }
                    )
                ));

                // Append the horizontal line paragraph to the body
                body.Append(horizontalLineParagraph);


                if (printChioces.HasSoal)
                {
                    var dataList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.Soval);
                    foreach (var item in dataList.Items)
                    {
                        item.CommissionTitle = await getCommissionTitleById(((SovalDto)item.MyData).Commission);
                    }
                    string[] headers = { "ردیف", "موضوع", "کمیسیون", "وضعیت" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                        item => item.CommissionTitle,
                        item => ((SovalDto)(item.MyData)).QuestionStatusDesc.Name
                    };

                    Table table = ExportTools.CreateTable(headers, extractors, dataList.Items);
                    body.Append(ExportTools.CreateBoldTitle("سوالات :", "28"), table);
                }

                if (printChioces.HasTazakor)
                {
                    var dataList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.Tazakor);
                    var filteredList = dataList.Items.Where(item => ((TazakorDto)(item.MyData)).TazakorType == TazakorEnum.Katbi).ToList();
                    string[] headers = { "ردیف", "موضوع", "وضعیت" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                        item => ((TazakorDto)(item.MyData)).PasokhNo == null ? "در دست پیگیری" : "پاسخ داده شده"
                    };

                    Table table = ExportTools.CreateTable(headers, extractors, filteredList);
                    body.Append(ExportTools.CreateBoldTitle("تذکرات :‌", "28"), table);
                }

                // Add Mokatebes table
                if (printChioces.HasMokatebe)
                {
                    var mokatebeList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.Mokatebe);
                    if (!printChioces.HasAllMokatebeTypes)
                    {
                        if (!(printChioces.HasAsl90Mokatebe))
                        {
                            mokatebeList = mokatebeList.Items.Where(x => ((MokatebeRsltDto)x.MyData).MokatebeType != MokatebeTypeEnum.Asl90).ToTableResult();
                        }
                        if (!(printChioces.HasPeyNeveshtMokatebe))
                        {
                            mokatebeList = mokatebeList.Items.Where(x => ((MokatebeRsltDto)x.MyData).MokatebeType != MokatebeTypeEnum.PeyNevesht).ToTableResult();
                        }
                        if (!(printChioces.HasAddiMokatebe))
                        {
                            mokatebeList = mokatebeList.Items.Where(x => ((MokatebeRsltDto)x.MyData).MokatebeType != MokatebeTypeEnum.Adi).ToTableResult();
                        }

                    }
                    if (!printChioces.HasAllMokatebe)
                    {
                        if (!(printChioces.IsPosetive))
                        {
                            mokatebeList = mokatebeList.Items.Where(x => ((MokatebeRsltDto)x.MyData).VaziatPasokh != ResponseStatusEnum.Mosbat).ToTableResult();
                        }
                        if (!(printChioces.IsNegative))
                        {
                            mokatebeList = mokatebeList.Items.Where(x => ((MokatebeRsltDto)x.MyData).VaziatPasokh != ResponseStatusEnum.Manfi).ToTableResult();
                        }
                        
                        if (!(printChioces.IsNotAnswered))
                        {
                            mokatebeList = mokatebeList.Items.Where(x => ((MokatebeRsltDto)x.MyData).PasokhNo != null).ToTableResult();
                        }
                    }

                    string[] headers = { "ردیف", "موضوع", "معاونت مربوطه", "شماره نامه نماینده", "تاریخ نامه نماینده", "شماره دبیرخانه","تاریخ دبیرخانه", "توضیحات" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                        item => item.CategoryParentName,
                        item => ((MokatebeRsltDto)(item.MyData)).ShomareDabirkhane,
                        item => ((MokatebeRsltDto)(item.MyData)).TarikhDabirKhaneMarkaziStr,
                        item => ((MokatebeRsltDto)(item.MyData)).ShomareDabirkhaneMarkazi,
                        item => ((MokatebeRsltDto)(item.MyData)).TarikhDabirKhaneStr,
                        item => item.Description
                    };

                    // Create and append the "مکاتبات" table
                    Table table = ExportTools.CreateTable(headers, extractors, mokatebeList.Items);

                    body.Append(ExportTools.CreateBoldTitle("مکاتبات :", "28"), table);

                }

                if (printChioces.HasTahghighTafahos)
                {
                    var dataList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.TahghighTafahos);
                    string[] headers = { "ردیف", "موضوع", "کمیسیون" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                        item => item.CommissionTitle,
                    };

                    Table table = ExportTools.CreateTable(headers, extractors, dataList.Items);
                    body.Append(ExportTools.CreateBoldTitle("تحقیق و تفحص‌ها :", "28"), table);
                }

                if (printChioces.HasTarh)
                {
                    var dataList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.Tarh);
                    foreach (var item in dataList.Items)
                    {
                        item.CommissionTitle = await getCommissionTitleById(((TarhDto)item.MyData).RelatedComission);
                    }
                    string[] headers = { "ردیف", "موضوع", "کمیسیون" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                        item => item.CommissionTitle,
                    };

                    Table table = ExportTools.CreateTable(headers, extractors, dataList.Items);
                    body.Append(ExportTools.CreateBoldTitle("طرح‌ها :", "28"), table);
                }


                if (printChioces.HasEzharat)
                {
                    var ezharatList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.EzhaaratResaneee);
                    string[] headers = { "ردیف", "موضوع" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                    };

                    Table table = ExportTools.CreateTable(headers, extractors, ezharatList.Items);
                    body.Append(ExportTools.CreateBoldTitle("‌اظهارات رسانه‌ای :", "28"), table);
                }
                if (printChioces.HasNotgh)
                {
                    var dataList = await GetDataEntries(printChioces.Id, DataEntryTypeEnum.Notgh);
                    string[] headers = { "ردیف", "موضوع" };
                    List<Func<DataEntryDto, string>> extractors = new List<Func<DataEntryDto, string>>
                    {
                        item => item.Title,
                    };

                    Table table = ExportTools.CreateTable(headers, extractors, dataList.Items);
                    body.Append(ExportTools.CreateBoldTitle("نطق‌ها :", "28"), table);
                }

                Paragraph lastParagraph = ExportTools.CreateBoldTitle("دفتر امور مجلس", "28", JustificationValues.Right);
                SpacingBetweenLines spacing = new SpacingBetweenLines
                {
                    Before = "720" // The value is in twentieths of a point; 720 twips = 36 points = 0.5 inch
                };
                //ParagraphProperties paragraphProperties = lastParagraph.Elements<ParagraphProperties>().FirstOrDefault();
                //if (paragraphProperties == null)
                //{
                //    paragraphProperties = new ParagraphProperties();
                //    lastParagraph.PrependChild(paragraphProperties);
                //}
                paragraphProperties.Append(spacing);

                // Append the last paragraph to the body of the document
                body.Append(lastParagraph);
                            }

            ms.Position = 0;
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
        
        [RequestLimit(NoOfRequest = 50, Seconds = 5)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetSenator(string models, Guid? commissionId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetSenatorProfileQuery();
            query.Filter = Filter;
            query.CommissionId = commissionId;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<SenatorVm> GetSenatorById(Guid senatorId)
        {
            var query = new GetSenatorProfileQuery
            {
                UserId = senatorId,
            };
            var list = await Mediator.Send(query);
            var userQuery = new GetUserByIdQuery()
            {
                UserId = senatorId
            };
            var udate = await Mediator.Send(userQuery);
            var rslt = list.Items.SingleOrDefault();
            rslt.Email = udate.Email;


            SenatorVm senatorVm = new SenatorVm()
            {
                Id = senatorId,
                ProfilePhotoUrl = rslt.ProfilePhoto is not null ? _baseUrl + rslt.ProfilePhoto : null,
                ProfilePhotoStr = rslt.ProfilePhoto,
                HozeCityName = rslt.HozeCity.Name,
                CityName = rslt.City.Name,
                Name = rslt.Name,
                Username = udate.Username,
                Mobile = rslt.Mobile,
                ComissionMembership = rslt.ComissionMembership,
                ComissionMembershipTitle = await getCommissionTitleById(rslt.ComissionMembership),
                PersonalFavorites = rslt.PersonalFavorites,
                HozeCityId = rslt.HozeCity.Id,
                BirthCityParentId = rslt.BirthCity.ParentId,
                PoliticalTending = rslt.PoliticalTending,
                CityId = rslt.City.Id,
                Email = rslt.Email,
                FractionMembership = rslt.FractionMembership,
                SocialActivity = rslt.SocailActivity,
                SenaHistory = rslt.SenaHistory,
                SabegheHeyatReise = rslt.SabegheHeyatReise,
                SabegheEmzaEstizah = rslt.SabegheEmzaEstizah,
                Reshte = rslt.Reshte,
                BirthdateString = rslt.BirthDate.ToPersianDate(),
                Birthdate = rslt.BirthDate,
                JobHistory = rslt.JobHistory,
                HozeEntekhabiInt = (int)rslt.HozeEntekhabi,
                ShoghleGhalebInt = (int)rslt.ShoghleGhaleb,
                MadrakTahsiliInt = (int)rslt.MadrakTahsili,
                MahaleTahsilInt = (int)rslt.MahaleTahsil,
                GerayeshSiasiInt = (int)rslt.GerayeshSiasi,
            };
            return senatorVm;
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid senatorId)
        {
            var senatorData = await GetSenatorById(senatorId);
            return View(senatorData);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid senatorId)
        {
            var senatorData = await GetSenatorById(senatorId);
            return View(senatorData);
        }

        public IActionResult GetHozeEntekhabiForDropDown()
        {
            return Json(HozeEntekhabiEnumHelper.GetList());
        }

        public IActionResult GetShoghleGhalebForDropDown()
        {
            return Json(ShoghleGhalebEnumHelper.GetList());
        }

        public IActionResult GetMadrakTahsiliForDropDown()
        {
            return Json(MadrakTahsiliEnumHelper.GetList());
        }

        public IActionResult GetMahaleTahsilForDropDown()
        {
            return Json(MahaleTahsilEnumHelper.GetList());
        }

        public IActionResult GetGerayeshSiasiForDropDown()
        {
            return Json(GerayeshSiasiEnumHelper.GetList());
        }

        private const long MaxFileSize = 10L * 1024L * 1024L; // 10MB, adjust to your need
        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        [HttpPost]
        public async Task<IActionResult> Create(SenatorVm request, CancellationToken cancellationToken)
        {

            var userCommand = new AddUserCommand()
            {
                Name = request.Name,
                Mobile = request.Mobile,
                Username = request.Username,
                Password = request.Password,
                CityId = request.CityId,
                Role = RoleTypeEnum.Senator,
                Email = request.Email,
            };
            var command = request.ConvertToCommand();
            command.UserId = await Mediator.Send(userCommand, cancellationToken);
            command.BirthDate = DateTime.Now;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Senator") });
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> Edit(SenatorVm request, CancellationToken cancellationToken)
        {

            var userCommand = new UpdateUserCommand()
            {
                UserId = request.Id,
                Name = request.Name,
                Mobile = request.Mobile,
                Password = request.EditPassword,
                CityId = request.CityId,
                Role = RoleTypeEnum.Senator,
                Email = request.Email,
            };
            await Mediator.Send(userCommand, cancellationToken);
            var command = request.ConvertToCommand();
            command.BirthDate = request.BirthdateString.ToMiladiDate();
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Senator") });
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Delete(Guid senatorId)
        {
            var vm = await GetSenatorById(senatorId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(SenatorDetailDto senatorData, CancellationToken cancellationToken)
        {
            var command = new DeleteUseCommand
            {
                UserId = senatorData.UserId
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [Auth]
        public async Task<IActionResult> GetSenatorsForDropDown(string models)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetSenatorProfileQuery();
            if (Filter.Filter == null || Filter.Filter?.Filters.ToList().Count != 0)
            {
                query.Filter = Filter;
            }
            else
            {
                query.Filter.Filter = null;
            }
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        public async Task<IActionResult> GetSenatorsForMultiSelect(string models)
        {
            var query = new GetSenatorProfileQuery();
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

    }
}