namespace WebAppSportsLeagueTestTask.WEB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebAppSportsLeagueTestTask.WEB.EFModels;
    using WebAppSportsLeagueTestTask.WEB.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAppSportsLeagueTestTask.WEB.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebAppSportsLeagueTestTask.WEB.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Movies.Any())
            {
                Director director1 = context.Directors.Add(new Director { FullName = "������� ���� �������" });
                Director director2 = context.Directors.Add(new Director { FullName = "����� ��������" });
                Director director3 = context.Directors.Add(new Director { FullName = "������ ������" });
                Director director4 = context.Directors.Add(new Director { FullName = "������ �������" });
                context.SaveChanges();

                context.Movies.Add(new Movie
                {
                    Director = director1,
                    Name = "����������� �������",
                    Year = 1979,
                    Description = "�� ����� ����� �� �������� ��������� ������������ ����� �� ���� � �������� � �������� ����� � ����� ���������������� ����������," +
                    " ���������� � ���������� ������ ����� ����� ������������ ����������� �������. �� ���� �� ���������� ���������� ���� ������ �����.",
                    Poster = "apokalipsis_segodnya.jpg"
                });
                context.Movies.Add(new Movie
                {
                    Director = director2,
                    Name = "��������� �������",
                    Year = 1974,
                    Description = "����� ��������� ����������� ���������� ������� ��������� �������� �������������� ������������� ������� ������ �� ������� �� ���� � ��������. " +
                    "������������ �� ��� ����, ����� �� ����������, ��� �������� � ������ ������ ���������, ���������� ��������� � ������� ���������, " +
                    "������ � ������� �������� ������� ����� ������ � ����� ����������� ���������",
                    Poster = "kitajskij_kvartal.jpg"
                });

                context.Movies.Add(new Movie
                {
                    Director = director3,
                    Name = "����� �������",
                    Year = 2014,
                    Description = "�������� ���������� ������� ������, � ��� ������� ��� ����� � �������, ������� ������, ��������� � �������������, �������. " +
                    "� ��� ��� ���, �� ���� ����, ��������� ������ � ��� � ����� �������� ����� � ����������� ���������� ���������� ������ � ���������� ����������, "+
                    "������� ����� ��� ����� ����� �� ������������ �� ����. � � ��������, � ������� ���������� ����, ��� ����� ������ ���� ����� ������� �������� � "+
                    "�������� �� ��������� ��������� ���� �� ����� ������ Audi �� ��������� ������ � ����� �������� ������ Peugeot, �� �������� �� ����� �������������� "+
                    "����� ����� �� ��������. � ��� ��� �������� ���� �������������� ������������� �� �� ������",
                    Poster= "�����_�������.jpg"
                });

                context.Movies.Add(new Movie
                {
                    Director = director4,
                    Name = "��������� ����",
                    Year = 2017,
                    Description = "� ������� �� ���������� ��������� ���������� ��������� ���� ����� ���� ������� ������� ����������� ���� ������� �������. " +
                    "���������� ����� ���, ������� �� ������� � �������� ���������� ��������� � ��������, ������ �������� �� ������������ ����������� � ����� " +
                    "������ ������� ����� �������.",
                    Poster = "vetrennaya_reka.jpg"
                });

                context.SaveChanges();
            }
        }
    }
}
