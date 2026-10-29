using Edusaz.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edusaz.Infrastructure.Migrations
{
    // Data-only corrections to universities entered through the admin panel. A migration runs exactly once, so a value
    // an admin sets later (even the same "45%") is never overwritten again. Each statement also matches the wrong value,
    // so a field that was already corrected by hand is left alone.
    [DbContext(typeof(EdusazDbContext))]
    [Migration("20261029120000_FixUniversityData")]
    public partial class FixUniversityData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // "45%" was the admin form's placeholder and got saved for nearly every university; the real rates are unknown.
            migrationBuilder.Sql(@"UPDATE ""Universities"" SET ""AcceptanceRate"" = '' WHERE ""AcceptanceRate"" = '45%';");

            // The ranking was copied from an Azerbaijani university.
            FixUniversity(migrationBuilder, "Ilia State University", @"""Ranking""", "'#1 Azərbaycanda'", "'#1 Gürcüstanda'");
            FixUniversity(migrationBuilder, "University of Amsterdam (UvA)", @"""Ranking""", "'#1 Azərbaycanda'", "'#1 Niderlandda'");

            // Founded in 1386; the digits were swapped.
            FixUniversity(migrationBuilder, "Heidelberg University", @"""EstablishedYear""", "1836", "1386");

            FixUniversity(migrationBuilder, "Technical University of Munich (TUM)", @"""Tuition""", "'0'", "'2,300-6,800 EUR / il'");
        }

        private static void FixUniversity(MigrationBuilder migrationBuilder, string name, string column, string wrong, string right)
        {
            migrationBuilder.Sql($@"
                UPDATE ""Universities"" SET {column} = {right}
                WHERE {column} = {wrong}
                  AND ""Id"" IN (SELECT ""UniversityId"" FROM ""UniversityTranslations"" WHERE ""Name"" = '{name}');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
