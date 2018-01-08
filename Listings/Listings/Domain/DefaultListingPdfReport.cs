using Listings.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class DefaultListingPdfReport
    {
        private Listing _listing;

        private Document _doc;
        public Document Document
        {
            get
            {
                PrepareDocument();
                return _doc;
            }
        }


        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }


        private DefaultListingPdfReportSetting _setting;
        public DefaultListingPdfReportSetting Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }


        private float _tableBordersWidth = 0.75f;
        private Color _tableBordersColor = new Color(51, 51, 51);


        public DefaultListingPdfReport(Listing listing)
        {
            _listing = listing;

            _setting = new DefaultListingPdfReportSetting();
        }


        private Document PrepareDocument()
        {
            _doc = new Document();

            Section section = PrepareDefaultDocumentStyles(_doc);

            // 24 column grid :D
            float sectionWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;
            Dictionary<string, float> grid = GetGridColumns(sectionWidth, 24);

            CreateInfoTable(_listing, section, grid);
            CreateDataTable(_listing, section, grid);
            CreateHoursSummaryTable(_listing, section, grid);
            CreateFinalTable(_listing, section, grid);

            return _doc;
        }


        private Section PrepareDefaultDocumentStyles(Document doc)
        {
            doc.Info.Title = string.Format("{0} {1}", Date.Months[12 - _listing.Month], _listing.Year);
            doc.Info.Author = "Evidoo app";

            Style style = doc.Styles["Normal"];
            style.Font.Name = "Calibri";
            style.Font.Size = 10;
            style.Font.Color = _tableBordersColor;

            PageSetup defaultPageSetup = doc.DefaultPageSetup.Clone();
            defaultPageSetup.PageFormat = PageFormat.A4;

            Section section = doc.AddSection();
            section.PageSetup = defaultPageSetup;

            section.PageSetup.TopMargin = new Unit(25);
            section.PageSetup.RightMargin = new Unit(20);
            section.PageSetup.BottomMargin = new Unit(25);
            section.PageSetup.LeftMargin = new Unit(25);

            return section;
        }


        private void CreateInfoTable(Listing listing, Section section, Dictionary<string, float> grid)
        {
            Table table = section.AddTable();
            //table.Borders.Color = new Color(30, 30, 30);
            //table.Borders.Width = 0.6;

            Column column = table.AddColumn(grid["col3"]);
            column = table.AddColumn(grid["col3"]);
            column = table.AddColumn(grid["col4"]);
            column = table.AddColumn(grid["col4"]);
            column = table.AddColumn(grid["col3"]);
            column = table.AddColumn(grid["col3"]);
            column = table.AddColumn(grid["col2"]);
            column = table.AddColumn(grid["col2"]);

            Row row = table.AddRow();
            row.Format.Font.Bold = true;
            Paragraph p = row.Cells[0].AddParagraph(string.Format("{0} {1}", Date.Months[12 - listing.Month], listing.Year));
            p.Format.Font.Size = 18;
            p.Format.Font.Bold = true;
            row.Cells[0].MergeRight = 1;
            row.Cells[0].MergeDown = 1;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            //row.Cells[1].AddParagraph("abc");
            row.Cells[2].AddParagraph("Zaměstnavatel:");
            row.Cells[2].MergeRight = 2;
            //row.Cells[3].AddParagraph("a");
            //row.Cells[4].AddParagraph("abc");
            row.Cells[5].AddParagraph("Jméno:");
            row.Cells[5].MergeRight = 2;
            //row.Cells[6].AddParagraph("abc");
            //row.Cells[7].AddParagraph("abc");

            row = table.AddRow();
            row.Height = 17;
            //row.TopPadding = 15;
            //row.BottomPadding = 15;
            //row.Cells[0].AddParagraph("abc");
            //row.Cells[1].AddParagraph("abc");
            row.Cells[2].AddParagraph(Setting.IsEmployerVisible ? (listing.Employer != null ? listing.Employer.Name : string.Empty) : string.Empty);
            row.Cells[2].MergeRight = 2;
            row.Cells[2].Borders.Color = _tableBordersColor;
            row.Cells[2].Borders.Width = _tableBordersWidth;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            //row.Cells[3].AddParagraph("a");
            //row.Cells[4].AddParagraph("abc");
            row.Cells[5].AddParagraph(Setting.IsOwnerNameVisible ? (string.IsNullOrEmpty(OwnerName) ? string.Empty : OwnerName) : string.Empty);
            row.Cells[5].MergeRight = 2;
            row.Cells[5].Borders.Color = _tableBordersColor;
            row.Cells[5].Borders.Width = _tableBordersWidth;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            //row.Cells[6].AddParagraph("abc");
            //row.Cells[7].AddParagraph("abc");

            // -----

            p = section.AddParagraph();
            p.Format.Font.Size = 2;
            p.Format.SpaceAfter = 3;
        }


        private void CreateDataTable(Listing listing, Section section, Dictionary<string, float> grid)
        {
            Table table = section.AddTable();
            table.Borders.Color = _tableBordersColor;
            table.Borders.Width = _tableBordersWidth;

            // Day Number
            Column column = table.AddColumn(grid["col2"]);
            // Day Short name
            column = table.AddColumn(grid["col1"]);
            // Locality
            column = table.AddColumn(grid["col9"]);
            // Shift Time
            column = table.AddColumn(grid["col3"]);
            // Lunch Time
            column = table.AddColumn(grid["col3"]);
            // Worked Time
            column = table.AddColumn(grid["col3"]);
            // Other Hours
            column = table.AddColumn(grid["col3"]);

            foreach (Column c in table.Columns) {
                c.Format.Alignment = ParagraphAlignment.Center;
            }

            Row row = table.AddRow();
            row.Height = 18;
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("Datum");
            row.Cells[1].AddParagraph("Den");
            row.Cells[2].AddParagraph("Popis práce - místo");
            row.Cells[3].AddParagraph("Prac. doba");
            row.Cells[4].AddParagraph("Oběd");
            row.Cells[5].AddParagraph("Odpr. hod.");
            row.Cells[6].AddParagraph("Ost. hod.");

            foreach (Cell c in row.Cells) {
                c.VerticalAlignment = VerticalAlignment.Center;
            }

            List<DayItem> items = PrepareDayItems(listing);
            foreach (DayItem item in items) {
                row = table.AddRow();
                row.Height = 18;
                row.Cells[0].AddParagraph(string.Format("{0}.", item.Day.ToString()));
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[1].AddParagraph(item.ShortDayName.ToUpper());
                row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

                if (item.IsWeekendDay) {
                    row.Cells[0].Format.Shading.Color = new Color(229, 229, 229);
                    row.Cells[0].Shading.Color = new Color(229, 229, 229);
                    row.Cells[1].Format.Shading.Color = new Color(229, 229, 229);
                    row.Cells[1].Shading.Color = new Color(229, 229, 229);
                }

                row.Cells[2].AddParagraph(item.Locality ?? string.Empty);
                row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[3].AddParagraph(item.TimeSetting != null ? GetHoursAndMinutesRange(item.TimeSetting.Start, item.TimeSetting.End, !item.TimeSetting.HasNoTime) : string.Empty);
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[4].AddParagraph(item.TimeSetting != null ? GetHoursAndMinutesRange(item.TimeSetting.LunchStart, item.TimeSetting.LunchEnd, !item.TimeSetting.HasNoTime, "-") : string.Empty);
                row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[5].AddParagraph(item.TimeSetting != null ? GetHoursAndMinutes(item.TimeSetting.TotalWorkedHours) : string.Empty);
                row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[6].AddParagraph(item.TimeSetting != null ? GetHoursAndMinutes(item.TimeSetting.OtherHours) : string.Empty);
                row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
            }

            // -----

            Paragraph p = section.AddParagraph();
            p.Format.Font.Size = 2;
            p.Format.SpaceAfter = 3;
        }


        private void CreateHoursSummaryTable(Listing listing, Section section, Dictionary<string, float> grid)
        {
            Table table = section.AddTable();
            table.Borders.Width = _tableBordersWidth;
            table.Borders.Color = _tableBordersColor;

            Column column = table.AddColumn(grid["col4"]);
            column = table.AddColumn(grid["col4"]);
            column = table.AddColumn(grid["col4"]);
            column = table.AddColumn(grid["col3"]);
            column = table.AddColumn(grid["col3"]);
            column = table.AddColumn(grid["col6"]);

            foreach (Column c in table.Columns) {
                c.Format.Alignment = ParagraphAlignment.Center;
            }

            // row 1
            Row row = table.AddRow();
            row.Height = 14;
            row.Cells[0].AddParagraph("Dovolená");
            row.Cells[1].AddParagraph(DisplayString(_listing.Vacation, Setting.IsVacationVisible));

            row.Cells[2].AddParagraph("Ostat. Hod.");
            row.Cells[3].AddParagraph(DisplayString(GetHoursAndMinutes(_listing.OtherHours), Setting.AreOtherHoursVisible));

            row.Cells[4].AddParagraph("Odprac. hod.");
            row.Cells[5].AddParagraph(DisplayString(GetHoursAndMinutes(_listing.WorkedHours), Setting.AreWorkedHoursVisible));

            // row 2
            row = table.AddRow();
            row.Height = 14;
            row.Cells[0].AddParagraph("Nemoc hod.");
            row.Cells[1].AddParagraph(DisplayString(_listing.SicknessHours, Setting.AreSiknessHoursVisible));

            row.Cells[2].AddParagraph("Svátek");
            row.Cells[3].AddParagraph(DisplayString(_listing.Holiday, Setting.AreHolidaysHoursVisible));

            row.Cells[4].AddParagraph("Obědy");
            row.Cells[5].AddParagraph(DisplayString(GetHoursAndMinutes(_listing.LunchHours), Setting.AreLunchHoursVisible));

            // row 3
            row = table.AddRow();
            row.Height = 20;
            Paragraph p = row.Cells[0].AddParagraph("Hodin celkem");
            p.Format.Font.Size = 12;
            p.Format.Alignment = ParagraphAlignment.Right;
            p.Format.Font.Bold = true;
            row.Cells[0].MergeRight = 4;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            p = row.Cells[5].AddParagraph(DisplayString(GetHoursAndMinutes(listing.TotalWorkedHours), Setting.AreTotalWorkedHoursVisible));
            p.Format.Font.Size = 16;
            p.Format.Alignment = ParagraphAlignment.Center;
            p.Format.Font.Bold = true;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

            // -----

            p = section.AddParagraph(string.Empty);
            p.Format.Font.Size = 2;
            p.Format.SpaceAfter = 3;
        }


        private void CreateFinalTable(Listing listing, Section section, Dictionary<string, float> grid)
        {
            Table table = section.AddTable();
            table.Borders.Width = _tableBordersWidth;
            table.Borders.Color = _tableBordersColor;

            Column column = table.AddColumn(grid["col6"]);
            column = table.AddColumn(grid["col6"]);
            column = table.AddColumn(grid["col6"]);
            column = table.AddColumn(grid["col6"]);

            // row 1
            Row row = table.AddRow();
            row.Height = 14;
            Paragraph p = row.Cells[0].AddParagraph("Základní mzda");
            p.Format.Font.Bold = true;
            p.Format.Font.Underline = Underline.Single;

            row.Cells[1].AddParagraph(DisplayString(string.Format("{0} Kč/h", listing.HourlyWage != null ? listing.HourlyWage.ToString() : string.Empty), Setting.IsHourlyWageVisible));
            row.Cells[2].AddParagraph("Dovolená dni");
            row.Cells[3].AddParagraph(DisplayString(_listing.VacationDays, Setting.AreVacationDaysVisible));

            // row 2
            row = table.AddRow();
            row.Height = 14;
            row.Cells[0].AddParagraph("Diety");
            row.Cells[1].AddParagraph(DisplayString(_listing.Diets, Setting.AreDietsVisible));
            row.Cells[2].AddParagraph("Placené svátky");
            row.Cells[3].AddParagraph(DisplayString(_listing.PaidHolidays, Setting.ArePaidHolidaysVisible));

            // row 3
            row = table.AddRow();
            row.Height = 14;
            row.Cells[0].AddParagraph("Odměny");
            row.Cells[1].AddParagraph(DisplayString(_listing.Bonuses, Setting.AreBonusesVisible));
            row.Cells[2].AddParagraph("$");
            row.Cells[3].AddParagraph(DisplayString(_listing.Dollars, Setting.AreDollarsVisible));

            // row 3
            row = table.AddRow();
            row.Height = 14;
            row.Cells[0].AddParagraph("Zálohy");
            row.Cells[1].AddParagraph(DisplayString(_listing.Prepayment, Setting.IsPrepaymentVisible));
            row.Cells[2].AddParagraph("Nemoc");
            row.Cells[3].AddParagraph(DisplayString(_listing.Sickness, Setting.IsSicknessVisible));

            // row 4
            row = table.AddRow();
            row.Height = 20;
            row.Cells[0].MergeRight = 2;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            p = row.Cells[0].AddParagraph("Podpis pracovníka");
            p.Format.Font.Size = 12;
            p.Format.Font.Bold = true;
            p.Format.Alignment = ParagraphAlignment.Right;

            foreach (Column c in table.Columns) {
                c.Format.Alignment = ParagraphAlignment.Center;
            }

            row.Cells[1].AddParagraph();
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
        }


        private string GetHoursAndMinutes(Time time)
        {
            string hours = time.Hours == 0 ? string.Empty : string.Format("{0}h", time.Hours.ToString());
            string minutes = time.Minutes == 0 ? string.Empty : string.Format(" {0}m", time.Minutes.ToString());
            return string.Format("{0}{1}", hours, minutes);
        }


        private string GetHoursAndMinutesRange(Time start, Time end, bool displayNoTime, string replaceNoTimeBy = null)
        {
            if (start == 0 && end == 0) {
                if (displayNoTime == false) {
                    return string.Empty;
                }

                if (string.IsNullOrEmpty(replaceNoTimeBy)) {
                    return string.Empty;
                } else {
                    return replaceNoTimeBy;
                }
            }

            return string.Format("{0} - {1}", start.HoursAndMinutes, end.HoursAndMinutes);
        }


        private List<DayItem> PrepareDayItems(Listing listing)
        {
            List<DayItem> dayItems = new List<DayItem>();

            int daysInMonth = listing.DaysInMonth;
            for (int day = 0; day < daysInMonth; day++) {
                ListingItem listingItem = listing.GetItemByDay(day + 1);
                DayItem dayItem;
                if (listingItem == null) {
                    dayItem = new DayItem(listing, day + 1);
                } else {
                    dayItem = new DayItem(listingItem);
                }

                dayItems.Add(dayItem);
            }

            return dayItems;
        }


        private string DisplayString(string s, bool isVisible)
        {
            if (isVisible == true) {
                if (string.IsNullOrEmpty(s)) {
                    return string.Empty;
                }
                return s;
            }

            return string.Empty;
        }


        private Dictionary<string, float> GetGridColumns(float totalWidth, int noColumns)
        {
            Dictionary<string, float> grid = new Dictionary<string, float>();
            for (int i = noColumns; i >= 1; i--) {
                grid.Add(string.Format("col{0}", i), totalWidth / ((float)noColumns / i));
            }

            return grid;
        }
    }
}
