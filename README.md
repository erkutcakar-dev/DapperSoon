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
```
Sidebar Navigation:
├── 🏠 Dashboard (Ana sayfa)
├── 👥 Player Statistics (Oyuncu istatistikleri)  
├── 🏟️ Team Statistics (Takım istatistikleri)
└── ⚖️ Fair Play Table (Disiplin tablosu)
```

### **Profile Integration**
- **Admin Profile**: Futbol topu ikonu ile kişiselleştirme
- **Responsive Design**: Mobil ve desktop uyumlu hamburger menü
- **User Context**: "Erkut Çakar - Football Analyst"

### **Tema Özelleştirmeleri**

#### **Custom Pagination Styling**
```css
.pagination .page-item .page-link {
    background-color: #495057 !important;
    border-color: #495057 !important;
    color: #fff !important;
}

.pagination .page-item.active .page-link {
    background-color: #6c757d !important;
    border-color: #6c757d !important;
}
```

#### **Badge Color System**
```css
Badge Types:
├── Success (Goals): #28a745
├── Info (Shots on Target): #17a2b8
├── Danger (Red Cards): #dc3545
├── Warning (Yellow Cards): #ffc107
├── Primary (Fouls): #007bff
└── Secondary (Fair Play): #6c757d
```

## 📈 İstatistik Hesaplama Algoritmaları

### **Oyuncu Metrikleri**

#### **Expected Goals (xGoals) Calculation**
```csharp
// Precision formatting for xGoals
xGoals.ToString("F2") // Output: "1.45"
```

#### **Primary Position Logic**
```sql
-- En çok oynadığı pozisyon belirleme
WITH PlayerMinutes AS (
    SELECT playerID, position, SUM(minutes) as total_minutes
    FROM appearances 
    WHERE minutes > 0
    GROUP BY playerID, position
)
SELECT TOP 1 position 
FROM PlayerMinutes 
WHERE playerID = @PlayerId 
ORDER BY total_minutes DESC
```

### **Takım Performans Metrikleri**

#### **Shot Efficiency Calculation**
```csharp
public decimal CalculateShootingAccuracy(int shotsOnTarget, int totalShots)
{
    if (totalShots == 0) return 0;
    return Math.Round((decimal)shotsOnTarget / totalShots * 100, 2);
}
```

#### **Goal Conversion Rate**
```csharp
public decimal CalculateGoalConversion(int goals, int shotsOnTarget)
{
    if (shotsOnTarget == 0) return 0;
    return Math.Round((decimal)goals / shotsOnTarget * 100, 2);
}
```

### **Fair Play Scoring System**

#### **Weighted Penalty System**
```csharp
public class FairPlayCalculator 
{
    private const int FOUL_WEIGHT = 1;
    private const int YELLOW_CARD_WEIGHT = 2;
    private const int RED_CARD_WEIGHT = 5;
    
    public int CalculateFairPlayScore(int fouls, int yellowCards, int redCards)
    {
        return (fouls * FOUL_WEIGHT) + 
               (yellowCards * YELLOW_CARD_WEIGHT) + 
               (redCards * RED_CARD_WEIGHT);
    }
}
```

## 🔧 Gelişmiş Özellikler

### **Filtreleme Sistemi**

#### **Multi-Parameter Search Implementation**
```csharp
// Dynamic WHERE clause generation
var whereConditions = new List<string>();
var parameters = new DynamicParameters();

if (!string.IsNullOrEmpty(search))
{
    whereConditions.Add("p.name LIKE @Search");
    parameters.Add("Search", $"%{search}%");
}

if (!string.IsNullOrEmpty(position))
{
    whereConditions.Add("a.position = @Position");
    parameters.Add("Position", position);
}

string whereClause = whereConditions.Any() 
    ? "WHERE " + string.Join(" AND ", whereConditions)
    : "";
```

#### **Auto-Submit Form Implementation**
```javascript
// Instant filter updates
$('select[name="selectedSeason"]').on('change', function() {
    $(this).closest('form').submit();
});
```

### **Pagination System**

#### **Smart Navigation Logic**
```csharp
public class PaginationHelper
{
    public static IEnumerable<int> GetPageNumbers(int currentPage, int totalPages, int maxVisible = 5)
    {
        int start = Math.Max(1, currentPage - maxVisible / 2);
        int end = Math.Min(totalPages, start + maxVisible - 1);
        
        return Enumerable.Range(start, end - start + 1);
    }
}
```

#### **Parameter Preservation**
```html
<!-- Sayfa geçişlerinde filtreleri koruma -->
<a href="?pageNumber=@(page)&pageSize=@Model.PageSize&search=@Model.Search&league=@Model.League">
    @page
</a>
```

### **Chart Visualization**

#### **Responsive Chart Configuration**
```javascript
Chart.defaults.responsive = true;
Chart.defaults.maintainAspectRatio = false;

// Dynamic data binding
const chartData = @Html.Raw(Json.Serialize(Model.ChartData));
```

#### **Color Theme Integration**
```javascript
const colorPalette = {
    primary: '#007bff',
    success: '#28a745', 
    danger: '#dc3545',
    warning: '#ffc107',
    info: '#17a2b8'
};
```

## 🚀 Performans Optimizasyonları

### **Database Optimizations**

#### **Efficient Query Patterns**
```sql
-- Index-friendly query structure
SELECT t.name, 
       SUM(ts.goals) as TotalGoals,
       SUM(ts.shots) as TotalShots,
       AVG(CAST(ts.shotsOnTarget AS FLOAT) / NULLIF(ts.shots, 0)) as ShootingAccuracy
FROM teams t
INNER JOIN teamstats ts ON t.teamID = ts.teamID
WHERE ts.season = @Season
GROUP BY t.teamID, t.name
ORDER BY TotalGoals DESC
```

#### **Connection Management**
```csharp
// Using pattern for resource management
using var connection = new SqlConnection(connectionString);
connection.Open();

var result = await connection.QueryAsync<TeamStatDto>(query, parameters);
```

#### **Pagination at Database Level**
```sql
-- Server-side pagination
SELECT * FROM (
    SELECT ROW_NUMBER() OVER (ORDER BY goals DESC) as RowNum, *
    FROM PlayerStats
    WHERE season = @Season
) AS NumberedRows
WHERE RowNum BETWEEN @StartRow AND @EndRow
```

### **Frontend Optimizations**

#### **Lazy Loading Implementation**
```javascript
// Chart lazy loading
document.addEventListener('DOMContentLoaded', function() {
    if (document.getElementById('topScorersChart')) {
        initializeTopScorersChart();
    }
});
```

#### **CSS Optimization**
```css
/* Critical CSS inlining */
.main-panel { 
    margin-left: 260px; 
    transition: margin-left 0.25s ease; 
}

@media (max-width: 991px) {
    .main-panel { margin-left: 0; }
}
```

## 📊 Veri Modelleri

### **Core Data Transfer Objects**

```csharp
public class PlayerPerformanceDto
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string Team { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public decimal XGoals { get; set; }
    public decimal XAssists { get; set; }
    public int GamesPlayed { get; set; }
    public int MinutesPlayed { get; set; }
}

public class TeamPerformanceDto  
{
    public string TeamName { get; set; }
    public int Goals { get; set; }
    public int Shots { get; set; }
    public int ShotsOnTarget { get; set; }
    public int Fouls { get; set; }
    public int RedCards { get; set; }
    public decimal ShootingAccuracy => Shots > 0 ? (decimal)ShotsOnTarget / Shots * 100 : 0;
}

public class FairPlayDto
{
    public string TeamName { get; set; }
    public string League { get; set; }
    public int Fouls { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int FairPlayScore => Fouls + (YellowCards * 2) + (RedCards * 5);
}
```

## 🔒 Güvenlik ve En İyi Pratikler

### **SQL Injection Prevention**
```csharp
// Parameterized queries with Dapper
var parameters = new DynamicParameters();
parameters.Add("@Search", search);
parameters.Add("@Season", season);

var result = connection.Query<PlayerDto>(query, parameters);
```

### **Input Validation**
```csharp
public IActionResult FilteredList(string search, int pageNumber = 1, int pageSize = 10)
{
    // Parameter validation
    pageNumber = Math.Max(1, pageNumber);
    pageSize = Math.Min(50, Math.Max(5, pageSize));
    
    search = search?.Trim();
    if (search?.Length > 100) search = search.Substring(0, 100);
}
```

## 📱 Responsive Design

### **Mobile-First Approach**
```css
/* Mobile base styles */
.container-fluid { padding: 15px; }

/* Tablet breakpoint */
@media (min-width: 768px) {
    .container-fluid { padding: 30px; }
}

/* Desktop breakpoint */  
@media (min-width: 1200px) {
    .container-fluid { padding: 40px; }
}
```

### **Chart Responsiveness**
```javascript
// Responsive chart resizing
window.addEventListener('resize', function() {
    Chart.helpers.each(Chart.instances, function(instance) {
        instance.resize();
    });
});
```

## 🚀 Kurulum ve Çalıştırma

### **Sistem Gereksinimleri**
- .NET 9.0 SDK
- SQL Server 2019+
- Visual Studio 2022 veya VS Code
- Node.js (opsiyonel - theme build için)

### **Kurulum Adımları**

1. **Repository'yi klonlayın**
```bash
git clone https://github.com/username/DapperSoon.git
cd DapperSoon
```

2. **Veritabanı bağlantısını yapılandırın**
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FootballStats;Trusted_Connection=true;"
  }
}
```

3. **Bağımlılıkları yükleyin**
```bash
dotnet restore
```

4. **Projeyi çalıştırın**
```bash
dotnet run
```

5. **Tarayıcıda açın**
```
https://localhost:7206
```

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
