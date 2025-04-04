Bu proje, LinkedIn üzerindeki "Kolay Başvuru (Easy Apply)" özelliğini kullanan iş ilanlarına otomatik olarak başvuru yapan bir Selenium tabanlı C# botudur.

🚀 Özellikler
Belirlediğiniz anahtar kelimelere göre ilan içeriklerini filtreler (örn. "React", ".Net", "Remote").

İlgili iş ilanlarında otomatik olarak "Kolay Başvuru" butonuna tıklar.

Başvuru sürecindeki:

“İleri”, “İncele”, “Başvuruyu gönder” adımlarını tamamlar.

Form içerisinde zorunlu alanlar boş bırakılmışsa bunu algılar ve başvurudan çıkar.

Başvuru sonunda "Bitti" veya "Kapat" butonuna tıklayarak bir sonraki ilana geçer.

Tüm ilanlar bitince sayfalama (pagination) ile diğer sayfalara geçer.

🛠 Gereksinimler
.NET 8+ / .NET Core

Google Chrome

Selenium WebDriver

Selenium Extras (WaitHelpers)

⚠️ Uyarılar
LinkedIn kullanıcı verinizle birlikte çalışır. Kendi kullanıcı profilinize ait tarayıcı verisiyle başlatılmalıdır (--user-data-dir).
