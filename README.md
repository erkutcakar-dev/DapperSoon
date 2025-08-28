# 🏆 DapperSoon - Football Statistics Dashboard

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-blue.svg)
![Bootstrap](https://img.shields.io/badge/Bootstrap-4.6-purple.svg)
![Chart.js](https://img.shields.io/badge/Chart.js-3.9-orange.svg)

## 📋 Proje Genel Bakış

**DapperSoon**, ASP.NET Core MVC ve Dapper ORM kullanılarak geliştirilmiş kapsamlı bir futbol istatistik yönetim sistemidir. Corona Bootstrap teması ile modern, responsive ve kullanıcı dostu bir arayüze sahiptir. Profesyonel futbol liglerinin detaylı istatistiklerini analiz etmek ve görselleştirmek için tasarlanmıştır.

### 🎯 Temel Amaç
- Futbol takımları ve oyuncularının performans verilerini merkezi bir platformda toplamak
- İstatistikleri anlamlı grafikler ve tablolar ile görselleştirmek
- Gelişmiş filtreleme ve arama özellikleri ile veri analizini kolaylaştırmak
- Modern web teknolojileri ile hızlı ve güvenilir bir admin paneli sunmak

## 🚀 Özellikler

### 📊 Dashboard (Ana Sayfa)

#### **Dinamik Widget Sistemi**
- **Toplam Maç Sayısı**: Veritabanındaki tüm maçların sayısı
- **Toplam Oyuncu Sayısı**: Kayıtlı oyuncu sayısı
- **Toplam Gol Sayısı**: Tüm sezonlardaki toplam gol sayısı
- **Toplam Takım Sayısı**: Liglerdeki aktif takım sayısı
- **Ortalama Gol/Maç**: Maç başına düşen ortalama gol sayısı
- **Ortalama Şut/Maç**: Maç başına düşen ortalama şut sayısı
- **Ortalama İsabetli Şut Oranı**: Toplam şutlara göre isabetli şut yüzdesi
- **Ortalama Faul/Maç**: Maç başına düşen ortalama faul sayısı

#### **İnteraktif Grafikler**

**Top Scorers Chart (En İyi Golcüler)**
- Bar chart formatında görselleştirme
- Sezon bazında filtreleme özelliği
- En yüksek gol atan 10 oyuncu
- Responsive tasarım ile mobil uyumlu
- Chart.js kütüphanesi ile smooth animasyonlar

**Top Red Cards Chart (En Çok Kırmızı Kart)**
- Kırmızı temalı bar chart
- Takım bazında kırmızı kart sayıları
- Büyükten küçüğe sıralama
- Sezon filtreleme desteği
- Hover efektleri ile detaylı bilgi gösterimi

#### **Veri Tabloları**

**Team Statistics Table**
- Takım adı, gol sayısı, puan, maç sayısı
- Badge'li renk kodlaması sistemi
- Sıralama ve arama özellikleri
- Sezon filtreleme

**League Statistics Table**
- Liga bazında toplam istatistikler
- Varsayılan sezon: 2020
- Maç sayısı, gol ortalaması, takım sayısı
- Genişletilebilir veri yapısı

### 👥 Player Statistics (Oyuncu İstatistikleri)

#### **Gelişmiş Filtreleme Sistemi**
```
🔍 Arama Filtreleri:
├── Oyuncu Adı: Text bazlı arama
├── Pozisyon: GK, DEF, MID, ATT
├── Liga: Serie A, La Liga, Premier League, Bundesliga, Ligue 1
└── Sezon: 2014-2020 arası tüm sezonlar
```

#### **Detaylı İstatistik Metrikleri**
- **Goals**: Toplam gol sayısı
- **Assists**: Asist sayısı  
- **xGoals**: Beklenen gol sayısı (0.00 formatında)
- **xAssists**: Beklenen asist sayısı (0.00 formatında)
- **Games Played**: Oynadığı maç sayısı
- **Minutes Played**: Toplam oyun süresi (dakika)
- **Team**: Oynadığı takım bilgisi
- **Position**: En çok oynadığı pozisyon (dakika bazlı hesaplama)

#### **Akıllı Veri İşleme**
- **Duplicate Prevention**: Oyuncular sadece en çok oynadığı pozisyonda görünür
- **Primary Position Logic**: CTE ile dakika bazlı pozisyon belirleme
- **Precision Formatting**: xGoals ve xAssists için iki ondalık hassasiyet

#### **Pagination Özellikleri**
- Sayfa başına kayıt: 5, 10, 15, 20, 50 seçenekleri
- Varsayılan: 10 kayıt
- Akıllı sayfa navigasyonu
- "X to Y of Z entries" gösterimi

### 🏟️ Team Statistics (Takım İstatistikleri)

#### **Kapsamlı Filtreleme**
```sql
WHERE conditions:
├── Team Name LIKE '%search%'
├── League = @selectedLeague
└── Season = @selectedSeason
```

#### **Performance Metrikleri**
- **Goals**: Toplam gol sayısı (yeşil badge)
- **Shots**: Toplam şut sayısı
- **Shots on Target**: İsabetli şut sayısı (mavi badge)  
- **Fouls**: Faul sayısı
- **Red Cards**: Kırmızı kart sayısı (kırmızı badge)

#### **İstatistiksel Hesaplamalar**
- **Shot Accuracy**: (Shots on Target / Total Shots) × 100
- **Goal Conversion**: (Goals / Shots on Target) × 100
- **Discipline Index**: (Red Cards + Yellow Cards) / Games Played

### ⚖️ Fair Play Table (Disiplin Tablosu)

#### **Disiplin Metrikleri**
- **Fouls**: Toplam faul sayısı (mavi badge - `badge-primary`)
- **Yellow Cards**: Sarı kart sayısı (sarı badge - `badge-warning`)
- **Red Cards**: Kırmızı kart sayısı (kırmızı badge - `badge-danger`)

#### **Fair Play Score Hesaplama**
```csharp
Fair Play Score = Fouls + (Yellow Cards × 2) + (Red Cards × 5)
```

#### **Renk Kodlaması Sistemi**
```css
Fair Play Score Badges:
├── 0-20 puan: badge-light (İyi - Açık gri)
├── 21-50 puan: badge-secondary (Orta - Gri)  
└── 51+ puan: badge-dark (Kötü - Siyah)
```

## 🛠️ Teknik Altyapı

### **Backend Architecture**

#### **Framework & ORM**
- **ASP.NET Core 9.0 MVC**: Modern web framework
- **Dapper ORM**: Yüksek performanslı micro-ORM
- **SQL Server**: Enterprise veritabanı yönetimi
- **Dynamic Objects**: Esnek veri modelleme yapısı
- **LINQ**: Güçlü veri manipülasyonu

#### **Database Schema**
```sql
Database Tables:
├── Teams (teamID, name, founded)
├── Players (playerID, name, dateOfBirth)  
├── Appearances (playerID, gameID, goals, assists, minutes)
├── TeamStats (teamID, season, goals, shots, fouls)
├── Games (gameID, homeTeamID, awayTeamID, season)
└── Leagues (leagueID, name, country)
```

#### **Data Access Patterns**
```csharp
// CTE kullanımı - Oyuncu pozisyon belirleme
WITH PlayerPositions AS (
    SELECT playerID, position, 
           SUM(minutes) as TotalMinutes,
           ROW_NUMBER() OVER (PARTITION BY playerID ORDER BY SUM(minutes) DESC) as rn
    FROM appearances 
    GROUP BY playerID, position
)
```

### **Frontend Technologies**

#### **UI Framework & Theme**
- **Corona Bootstrap Theme**: Profesyonel admin teması
- **Bootstrap 4.6**: Responsive grid sistemi
- **Material Design Icons**: 2000+ ikon kütüphanesi
- **Custom CSS**: Özel pagination stilleri (koyu tema)

#### **JavaScript Libraries**
- **Chart.js 3.9**: İnteraktif grafik kütüphanesi
- **jQuery 3.6**: DOM manipülasyonu
- **Bootstrap JS**: Modal, dropdown, collapse bileşenleri

#### **Chart Configurations**
```javascript
// Bar Chart Example - Top Scorers
new Chart(ctx, {
    type: 'bar',
    data: {
        labels: playerNames,
        datasets: [{
            label: 'Goals',
            data: goalCounts,
            backgroundColor: 'rgba(54, 162, 235, 0.6)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});
```

## 🎨 UI/UX Özellikleri

### **Navigation Architecture**
- **Sidebar Navigation**: Dashboard, Player Statistics, Team Statistics, Fair Play Table
- **Admin Profile**: Futbol topu ikonu ile kişiselleştirme
- **Responsive Design**: Mobil ve desktop uyumlu hamburger menü
- **User Context**: "Erkut Çakar - Football Analyst"

### **Tema Özelleştirmeleri**
- **Custom Pagination**: Koyu tema ile uyumlu gri tonları
- **Badge Color System**: Anlamlı renk kodlaması (yeşil: gol, mavi: şut, kırmızı: kart)
- **Icon Integration**: Material Design Icons
- **Professional Layout**: Corona teması uyarlaması

## 📈 İstatistik Hesaplama Algoritmaları

### **Oyuncu Metrikleri**
- **Expected Goals (xGoals)**: İki ondalık hassasiyette formatlanmış beklenen gol sayısı
- **Primary Position Logic**: En çok dakika oynadığı pozisyon CTE ile belirlenir
- **Unique Players**: Duplicate kayıtlar engellenir

### **Takım Performans Metrikleri**
- **Shot Efficiency**: İsabetli şut yüzdesi hesaplaması
- **Goal Conversion**: Gol dönüşüm oranı analizi
- **Discipline Index**: Disiplin durumu değerlendirmesi

### **Fair Play Scoring System**
- **Weighted System**: Faul=1, Sarı kart=2, Kırmızı kart=5 puan
- **Season Filtering**: Sezona özel disiplin analizi
- **League Comparison**: Ligalar arası fair play karşılaştırması

## 🔧 Gelişmiş Özellikler

### **Filtreleme Sistemi**
- **Multi-Parameter Search**: Çoklu filtre desteği (oyuncu adı, pozisyon, liga, sezon)
- **Dynamic WHERE Clause**: Otomatik SQL sorgu oluşturma
- **Auto-Submit Forms**: Anında sonuç güncelleme
- **Parameter Preservation**: Sayfa geçişlerinde filtre korunması

### **Pagination System**
- **Smart Navigation**: Akıllı sayfa numaraları (maksimum 5 görünür)
- **Flexible Page Sizes**: 5, 10, 15, 20, 50 seçenekleri
- **Total Count Display**: "X to Y of Z entries" gösterimi
- **Icon Navigation**: Chevron ikonu ile kullanıcı dostu navigasyon

### **Chart Visualization**
- **Responsive Charts**: Mobil uyumlu grafik boyutlandırması
- **Dynamic Data Binding**: Model'den direkt veri entegrasyonu
- **Color Theme Integration**: Tutarlı renk paleti
- **Smooth Animations**: Chart.js ile akıcı geçişler

## 🚀 Performans Optimizasyonları

### **Database Optimizations**
- **Efficient Queries**: Index-friendly SQL sorgu yapıları
- **Connection Management**: Using pattern ile resource yönetimi
- **Server-side Pagination**: ROW_NUMBER() ile veritabanı seviyesinde sayfalama
- **Parameterized Queries**: SQL injection koruması ve plan cache optimizasyonu

### **Frontend Optimizations**
- **Lazy Loading**: Chart'lar sadece ihtiyaç olduğunda yüklenir
- **Critical CSS**: Önemli stillerin inline olarak yüklenmesi
- **Responsive Design**: Mobil-first yaklaşım ile performans
- **Minimal JavaScript**: Sadece gerekli JS kütüphaneleri

## 📊 Veri Modelleri

### **Core Data Transfer Objects**
- **PlayerPerformanceDto**: Oyuncu performans verileri (Name, Position, Team, Goals, Assists, xGoals, xAssists, Games, Minutes)
- **TeamPerformanceDto**: Takım performans metrikleri (TeamName, Goals, Shots, ShotsOnTarget, Fouls, RedCards, ShootingAccuracy)
- **FairPlayDto**: Disiplin tablosu verileri (TeamName, League, Fouls, YellowCards, RedCards, FairPlayScore)
- **DashboardWidgetDto**: Widget verileri (WidgetName, Value, Icon, Color, Description)
- **ChartDataDto**: Grafik veri yapısı (Labels, Datasets, Colors)

## 🔒 Güvenlik ve En İyi Pratikler

### **Security Measures**
- **SQL Injection Prevention**: Parameterized queries ile Dapper kullanımı
- **Input Validation**: Kullanıcı girişlerinin sanitize edilmesi
- **Parameter Validation**: Sayfa boyutu ve sayfa numarası kontrolü
- **Length Restrictions**: Arama metinleri için maksimum karakter sınırı

## 📱 Responsive Design

### **Mobile-First Approach**
- **Adaptive Layouts**: Mobile, tablet ve desktop için optimize edilmiş görünüm
- **Flexible Grid System**: Bootstrap 4.6 grid sistemi
- **Touch-Friendly**: Mobil dokunmatik deneyimi için optimize edilmiş butonlar
- **Chart Responsiveness**: Ekran boyutuna göre otomatik grafik yeniden boyutlandırma

## 🚀 Kurulum ve Çalıştırma

### **Sistem Gereksinimleri**
- .NET 9.0 SDK
- SQL Server 2019+
- Visual Studio 2022 veya VS Code
- Node.js (opsiyonel - theme build için)

### **Kurulum Adımları**

1. **Repository'yi klonlayın**: `git clone https://github.com/username/DapperSoon.git`
2. **Veritabanı bağlantısını yapılandırın**: `appsettings.json` dosyasında connection string'i güncelleyin
3. **Bağımlılıkları yükleyin**: `dotnet restore`
4. **Projeyi çalıştırın**: `dotnet run`
5. **Tarayıcıda açın**: `https://localhost:7206`

## 📈 Gelecek Geliştirmeler

### **Planlanmış Özellikler**
- [ ] Real-time data updates via SignalR
- [ ] Export functionality (PDF, Excel)
- [ ] Advanced analytics dashboard
- [ ] Multi-language support
- [ ] API endpoints for mobile app
- [ ] Machine learning predictions
- [ ] User role management
- [ ] Data visualization enhancements

### **Teknik İyileştirmeler**
- [ ] Redis caching implementation
- [ ] GraphQL API integration
- [ ] Microservices architecture
- [ ] Docker containerization
- [ ] CI/CD pipeline setup
- [ ] Performance monitoring
- [ ] Automated testing suite

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Branch'i push edin (`git push origin feature/amazing-feature`)
5. Pull Request açın

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

## 👨‍💻 Geliştirici

**Erkut Çakar** - Football Analyst & Developer

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!
