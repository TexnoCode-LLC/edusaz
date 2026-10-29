using System;
using Edusaz.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edusaz.Infrastructure.Migrations
{
    // Data-only: the scholarships page was empty, so add the main government scholarships once. Afterwards they are
    // managed from the admin panel like any other scholarship; one that already exists (by English name) is skipped.
    // Facts as of October 2026 — Closed + "notify" means the next call has not opened yet.
    [DbContext(typeof(EdusazDbContext))]
    [Migration("20261029120100_AddCoreScholarships")]
    public partial class AddCoreScholarships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            AddScholarship(migrationBuilder, "uk", "Birləşmiş Krallıq", "Closed", "notify", "",
                "100% təhsil haqqı + aylıq yaşayış xərci + aviabilet",
                "Chevening Təqaüdü",
                "Böyük Britaniya hökumətinin gələcək liderlər üçün tam maliyyələşdirilən, bir illik magistr təqaüdü. 2027/28 üçün müraciətlər 6 oktyabr 2026-da bağlandı; növbəti qəbul 2027-ci ilin avqustunda açılır.",
                "Ən azı 2 il iş təcrübəsi olan bakalavr məzunları",
                "Chevening Scholarship",
                "UK Government scholarship for future leaders, fully funding a one-year master's degree. Applications for 2027/28 closed on 6 October 2026; the next round opens in August 2027.",
                "Bachelor's graduates with at least 2 years of work experience");

            AddScholarship(migrationBuilder, "hu", "Macarıstan", "Closed", "notify", "2027-01-15",
                "Təhsil haqqı yoxdur + aylıq təqaüd + yataqxana + tibbi sığorta",
                "Stipendium Hungaricum",
                "Macarıstan hökumətinin bakalavr, magistr və doktorantura üçün təqaüdü: təhsil haqqından azadlıq, aylıq təqaüd, yataqxana yeri və tibbi sığorta. 2027/28 üçün müraciətlər 2026-cı ilin noyabrında açılır.",
                "Tərəfdaş ölkələrin (o cümlədən Azərbaycanın) vətəndaşları",
                "Stipendium Hungaricum",
                "Hungarian Government scholarship for bachelor's, master's and doctoral studies: tuition waiver, monthly stipend, dormitory place and medical insurance. Applications for 2027/28 open in November 2026.",
                "Citizens of partner countries, including Azerbaijan");

            AddScholarship(migrationBuilder, "de", "Almaniya", "Open", "check", "",
                "€992/ay + illik €460 təhsil yardımı + sığorta + yol xərci",
                "DAAD Magistr Təqaüdü",
                "Almaniyada magistr təhsili üçün DAAD təqaüdü. Son müraciət tarixi proqrama və ölkəyə görə dəyişir, adətən oktyabr–noyabr aylarındadır.",
                "Son 6 ildə bakalavrı bitirmiş, ən azı 2 il iş təcrübəsi olan məzunlar",
                "DAAD Master's Scholarship",
                "DAAD scholarship for master's studies in Germany. The deadline depends on the programme and country, usually October–November.",
                "Graduates who finished their bachelor's within the last 6 years, with at least 2 years of work experience");

            AddScholarship(migrationBuilder, null, "Fransa", "Open", "check", "2027-01-06",
                "Magistr: €1,200/ay, PhD: €2,100/ay + aviabilet + sığorta",
                "France Excellence Eiffel Təqaüdü",
                "Fransa Xarici İşlər Nazirliyinin magistr və doktorantura üçün təqaüdü. Tələbə birbaşa müraciət etmir — Fransa universiteti namizədi irəli sürür; universitetlərin daxili son tarixi daha tezdir.",
                "Fransa universitetinin namizəd göstərdiyi magistr və PhD tələbələri",
                "France Excellence Eiffel Scholarship",
                "French Ministry for Europe and Foreign Affairs scholarship for master's and PhD studies. Students do not apply directly — a French university nominates them, and universities set earlier internal deadlines.",
                "Master's and PhD students nominated by a French university");

            AddScholarship(migrationBuilder, "it", "İtaliya", "Closed", "notify", "",
                "€10,800/il (€1,200/ay) + təhsil haqqından azadlıq",
                "Invest Your Talent in Italy (meritə əsaslanan)",
                "İtaliya Xarici İşlər Nazirliyinin magistr tələbələri üçün meritə əsaslanan təqaüdü, İtaliya şirkətində təcrübə ilə. 2026/27 üçün müraciətlər 11 may 2026-da bağlandı; növbəti qəbul adətən yazda açılır.",
                "Yüksək akademik nəticələri olan beynəlxalq magistr namizədləri",
                "Invest Your Talent in Italy (merit-based)",
                "Merit-based scholarship from the Italian Ministry of Foreign Affairs for master's students, including an internship at an Italian company. Applications for 2026/27 closed on 11 May 2026; the next call usually opens in spring.",
                "International master's applicants with strong academic results");

            AddScholarship(migrationBuilder, "it", "İtaliya", "Open", "check", "",
                "İllik ~€7,000-dək + təhsil haqqından azadlıq + yataqxana/yemək",
                "İtaliya DSU Regional Təqaüdü (ehtiyaca əsaslanan)",
                "İtaliya regionlarının (ER.GO, DiSCo Lazio, DSU Toscana, EDISU və s.) ailə gəlirinə əsaslanan təqaüdü. Məbləğ və son tarix regiona görə dəyişir; ailə gəliri ISEE/ISPE ilə sənədləşdirilir.",
                "Ailə gəliri regional limitdən aşağı olan dövlət universiteti tələbələri",
                "Italy DSU Regional Scholarship (need-based)",
                "Need-based scholarship from Italian regional agencies (ER.GO, DiSCo Lazio, DSU Toscana, EDISU and others). Amount and deadline depend on the region; family income is documented through ISEE/ISPE.",
                "Public university students whose family income is below the regional limit");
        }

        private static void AddScholarship(MigrationBuilder migrationBuilder, string countryCode, string location, string status,
            string buttonType, string deadline, string amount, string nameAz, string descAz, string eligibleAz,
            string nameEn, string descEn, string eligibleEn)
        {
            var id = Guid.NewGuid();
            var country = countryCode == null ? "NULL" : $@"(SELECT ""Id"" FROM ""Countries"" WHERE ""Code"" = '{countryCode}' LIMIT 1)";

            migrationBuilder.Sql($@"
                INSERT INTO ""Scholarships""
                    (""Id"", ""Name"", ""Location"", ""CountryId"", ""UniversityId"", ""Status"", ""Amount"", ""Deadline"", ""Eligible"", ""Places"",
                     ""ButtonType"", ""IsDeleted"", ""CreatedDate"", ""LastUpdatedDate"", ""DeletedDate"")
                SELECT '{id}', {Q(nameAz)}, {Q(location)}, {country}, NULL, {Q(status)}, {Q(amount)}, {Q(deadline)}, {Q(eligibleAz)}, '',
                       {Q(buttonType)}, false, now(), now(), '0001-01-01'
                WHERE NOT EXISTS (SELECT 1 FROM ""ScholarshipTranslations"" WHERE ""Name"" = {Q(nameEn)});");

            AddTranslation(migrationBuilder, id, "az", nameAz, descAz, eligibleAz);
            AddTranslation(migrationBuilder, id, "en", nameEn, descEn, eligibleEn);
        }

        private static void AddTranslation(MigrationBuilder migrationBuilder, Guid scholarshipId, string lang, string name, string description, string eligible)
        {
            migrationBuilder.Sql($@"
                INSERT INTO ""ScholarshipTranslations""
                    (""Id"", ""ScholarshipId"", ""LanguageId"", ""Name"", ""Description"", ""Eligible"", ""IsDeleted"", ""CreatedDate"", ""LastUpdatedDate"", ""DeletedDate"")
                SELECT '{Guid.NewGuid()}', '{scholarshipId}', l.""Id"", {Q(name)}, {Q(description)}, {Q(eligible)}, false, now(), now(), '0001-01-01'
                FROM ""Languages"" l
                WHERE l.""Code"" = '{lang}' AND EXISTS (SELECT 1 FROM ""Scholarships"" WHERE ""Id"" = '{scholarshipId}');");
        }

        private static string Q(string value) => "'" + value.Replace("'", "''") + "'";

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
