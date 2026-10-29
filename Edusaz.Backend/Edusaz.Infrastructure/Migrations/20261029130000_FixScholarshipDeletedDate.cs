using Edusaz.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edusaz.Infrastructure.Migrations
{
    // AddCoreScholarships wrote DeletedDate as '0001-01-01', which Postgres reads in the server's time zone (+04), so it
    // lands before year 1 in UTC and Npgsql cannot read it back — the whole scholarships list failed. Npgsql stores
    // DateTime.MinValue as -infinity, so use that.
    [DbContext(typeof(EdusazDbContext))]
    [Migration("20261029130000_FixScholarshipDeletedDate")]
    public partial class FixScholarshipDeletedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var table in new[] { "Scholarships", "ScholarshipTranslations" })
            {
                migrationBuilder.Sql($@"
                    UPDATE ""{table}"" SET ""DeletedDate"" = '-infinity'
                    WHERE ""DeletedDate"" > '-infinity' AND ""DeletedDate"" < '0002-01-01 00:00:00+00';");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
