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
                Director director1 = context.Directors.Add(new Director { FullName = "Фрэнсис Форд Коппола" });
                Director director2 = context.Directors.Add(new Director { FullName = "Роман Полански" });
                Director director3 = context.Directors.Add(new Director { FullName = "Дамиан Сифрон" });
                Director director4 = context.Directors.Add(new Director { FullName = "Тейлор Шеридан" });
                context.SaveChanges();

                context.Movies.Add(new Movie
                {
                    Director = director1,
                    Name = "Апокалипсис сегодня",
                    Year = 1979,
                    Description = "Во время войны во Вьетнаме спецагент отправляется вверх по реке в Камбоджу с приказом найти и убить полусумасшедшего полковника," +
                    " создавшего в отдаленном районе нечто вроде собственного королевства насилия. По пути он становится свидетелем мира ужасов войны.",
                    Poster = "apokalipsis_segodnya.jpg"
                });
                context.Movies.Add(new Movie
                {
                    Director = director2,
                    Name = "Китайский квартал",
                    Year = 1974,
                    Description = "Гиттс принимает предложение загадочной богатой красавицы заняться расследованием обстоятельств тайного романа на стороне ее мужа — инженера. " +
                    "Согласившись на это дело, Гиттс не подозревал, что окажется в центре тайных скандалов, беспредела коррупции и скрытых махинаций, " +
                    "правда о которых всплывет однажды ночью вместе с телом несчастного инженера…",
                    Poster = "kitajskij_kvartal.jpg"
                });

                context.Movies.Add(new Movie
                {
                    Director = director3,
                    Name = "Дикие истории",
                    Year = 2014,
                    Description = "Гэбриела Пастернака обижали многие, с кем сводила его жизнь — девушка, учитель музыки, профессор в консерватории, дантист. " +
                    "И вот все они, на свою беду, оказались вместе с ним в одном самолете… Ночью в придорожной закусочной официантка узнает в посетителе ростовщика, "+
                    "который много лет назад довел до самоубийства ее отца. А у поварихи, в прошлом отсидевшей срок, для мести такому гаду сразу нашлось средство — "+
                    "крысиный яд… Городской красавчик ехал на своей мощной Audi по пустынной дороге и решил обогнать старый Peugeot, но сидевший за рулем провинциальный "+
                    "упырь решил не уступать. И вот уже кровавый итог бессмысленного соперничества не за горами…",
                    Poster= "Дикие_истории.jpg"
                });

                context.Movies.Add(new Movie
                {
                    Director = director4,
                    Name = "Ветренная река",
                    Year = 2017,
                    Description = "В пустыне на территории индейской резервации «Ветреная река» егерь Кори Ламберт находит изувеченное тело молодой девушки. " +
                    "Начинающий агент ФБР, которая не знакома с местными природными условиями и обычаями, просит охотника из Департамента рыболовства и охоты " +
                    "помочь поймать убийц девушки.",
                    Poster = "vetrennaya_reka.jpg"
                });

                context.SaveChanges();
            }
        }
    }
}
