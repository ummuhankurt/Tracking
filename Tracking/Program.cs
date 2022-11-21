using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking
{
    class Program
    {
        static async Task Main(string[] args)
        {
            KutphaneDbContext context = new();

            #region AsNoTracking Metodu
            //Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.
            /*ChangeTracker,takip ettiği nesnelerin sayısıyla orantılı olacak şekilde bir maliyete sahiptir.O yüzden
             üzerinde işlem yapılmayacak nesnelerin takibi gereksiz maliyettir.
             */
            //AsNoTracking metodu,context üzerinden sorgu neticesinde gelecek olan verilerin change tracker tarafından takip edilmesini engeller.
            //AsNoTracking metodu ile Change Tracker'in ihtiyaç olmayan verilerdeki maliyetini törpülemiş olabiliriz.
            //AsNoTracking ile yapılan sorgulamalarda, verileri elde edebilir,bu verileri istenilen noktalarda kullabilir lakin üzerinde değişiklik yapamayız.
            //var kullanicilar = await context.Kullanicilar.ToListAsync(); //Bu şekilde sadece listeleme yapacağım nesneler üzerinde herhangi bir takip işlemi yapmama gerek yok.
            //Peki nasıl takibi bırakıcaz? Şöyle;
            //var kullanicilar = await context.Kullanicilar.AsNoTracking().ToListAsync();
            //foreach (var kullanici in kullanicilar)
            //{
            //    Console.WriteLine(kullanici.Ad);
            //    kullanici.Ad = $"Yeni-{kullanici.Ad}";
            //    context.Kullanicilar.Update(kullanici); // Burada manuel olarak update işlemi yaptık. Ama sadece bu işlem veritabanında değişiklik yapmaz.SaveChanges()'ı çağırmalısın.
            //}
            //await context.SaveChangesAsync();
            // Alttaki sorguyu çalıştırmadan önce konfigürasyon olarak NoTracking() yapıldı. Bu yüzden veride değişim gerçekleşmedi.
            //var kullanici = await context.Kullanicilar.SingleOrDefaultAsync(k => k.KullaniciId == 1);
            //Console.WriteLine(kullanici.Ad);
            //kullanici.Ad = "EMELLLL";
            //await context.SaveChangesAsync();
            //var kullanicilar = await context.Kullanicilar.ToListAsync();
            //foreach (var item in kullanicilar)
            //{
            //    Console.WriteLine(item.Ad);
            //}
            #endregion

            #region AsNoTrackingWithIdentityResolution
            //AsNoTracking metodu ile yapılan sorgularda yinelenen datalar farklı instancelarda karşılanırlar.
            //Ne demek istiyorum? Örneğin Admin rolüne sahip 3 tane kullanıcı var.Hepsine 3 ayrı Admin instance'ı oluşturulur.
            //3 ayrı kullanıcı için tek bir Admin nesnesini kullanmamızı sağlayan yapı ChangeTrackerdir.
            //MALİYET OPTİMİZASYON AÇISINDAN ÇOK İYİ.
            //Hem ChangeTracker tarafından izlenmeyecek, hem de aynı role sahip kullanıcı rolünden birsürü oluşturmayacak.1 tane oluşturup onu kullanacak
            //var kitaplar = await context.Kitaplar.Include(k => k.Yazarlar).AsNoTrackingWithIdentityResolution().ToListAsync(); // Her kitap için ayrı ayrı yazar nesnesi oluşturmaz.Tek bir tane oluşturup onu kullanır.
            //var kitaplar = await context.Kitaplar.Include(k => k.Yazarlar).AsNoTracking().ToListAsync(); // Her kitap için ayrı ayrı yazar nesnesi oluşturur.
            //özellikle çoka çok ilişkilerde kullanılması maliyeti optimize etmemizi sağlar.
            //bir kullanıcının birden fazla rolü olabilir.Bir rol, birden fazla kullanıcıya ait olabilir. Çoka çok ilişki.
            /*var kullanicilar = await context.Kullanicilar.Include(k => k.Roller).AsNoTrackingWithIdentityResolution();
             * Örneğin Ayşe kullanıcısının iki tane rolü var; admin ve product manager. Ali kullanıcısının da admin rolü var. Kullanıcılar rolleri ile birlikte
             * çekilirken, admin nesnesinden Ali ve Ayşe için bir tane oluşturur. Product manager rolü için ise bir tane nesne oluşturur Ayşe için.
            */
            /*var kullanicilar = await context.Kullanicilar.Include(k => k.Roller).AsNoTracking();
             * CT devreye girmeyeceği için rol nesnesini daha önce üretmiş olmasına rağmen tekrar üretir.Yani Ayşe nesnesi için; Admin, Product Maanger
             * Ali için tekrar Admin nesnesi üretir.
             */
            #endregion

            #region AsTracking
            //Context üzerinden gelen dataların CT tarafından takip edilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.
            /*UseQueryTrackingBehavior metodunun davranışı gereği uygulama seviyesinde CT'ın default olarak devrede olup olmamasını ayarlıyoruz.
             * Eğer ki default olarak pasif hale getirirlirse böyle bir durumda takip mekanizmasının ihtiyaç olduğu sorgularda AsNoTracking fonksiyonunu
             * kullanabilir ve böylece takip mekanizmasını iradeli bir şekilde devreye sokmuş oluruz.
             */
            // var kitaplar = await context.Kitaplar.AsTracking().ToListAsync();
            #endregion

            #region UseQueryTrackingBehavior
            /*Uygulama(Ef Core) seviyesinde ilgili contextten gelen verilerin üzerinde CT mekanizmasının davranışını temel seviyede belirlememizi sağlar.
             * Yani konfigürasyon fonksiyonudur.
             * optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); NoTracking seçilirse takip edilmeyecektir.
             * optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution); Takip edilmeyecek ama, yinelenen dataların ayrı instancelarda olmasını engelleyecek.
             * optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll); Takip edilir. Default olarak bu çalaışır.
             */
            #endregion
        }
    }
}
