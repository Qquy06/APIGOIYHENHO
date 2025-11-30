using Google.Api;
using Google.Cloud.Firestore;
using MatchAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatchAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
       
      /*  private static readonly List<User> Users = new List<User>
        {
           new User
    {
        Id = "1",
        ten = "Ngọc Anh",
        tuoi = 22,
        gioitinh = "Nữ",
        snhat = "2002-05-15",
        hocvan = "Đại học Kinh tế",
        nghenghiep = "Marketing Intern",
        thoiquen = "Nghe nhạc, Cafe",
        gthieu = "Thích những nơi yên tĩnh và nhạc Acoustic.",
        chieucao = 1.60f,
        vitri = "Hà Nội",
        AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg",
        photos = new List<string> { "https://randomuser.me/api/portraits/women/1.jpg", "https://randomuser.me/api/portraits/women/10.jpg" },
        isVip = true
    },
    new User
    {
        Id = "2",
        ten = "Minh Châu",
        tuoi = 23,
        gioitinh = "Nữ",
        snhat = "2001-08-20",
        hocvan = "Đại học Ngoại Thương",
        nghenghiep = "Content Creator",
        thoiquen = "Du lịch, Chụp ảnh",
        gthieu = "Yêu màu hồng và sự ghét sự giả dối :D",
        chieucao = 1.65f,
        vitri = "TP. Hồ Chí Minh",
        AvatarUrl = "https://randomuser.me/api/portraits/women/2.jpg",
        photos = new List<string> { "https://randomuser.me/api/portraits/women/2.jpg" },
        isVip = false
    },
    new User
    {
        Id = "3",
        ten = "Quang Tuấn",
        tuoi = 24,
        gioitinh = "Nam",
        snhat = "2000-12-05",
        hocvan = "Đại học Bách Khoa",
        nghenghiep = "Developer",
        thoiquen = "Code, Game, Gym",
        gthieu = "Looking for player 2.",
        chieucao = 1.75f,
        vitri = "Đà Nẵng",
        AvatarUrl = "https://randomuser.me/api/portraits/men/3.jpg",
        photos = new List<string> { "https://randomuser.me/api/portraits/men/3.jpg", "https://randomuser.me/api/portraits/men/30.jpg" },
        isVip = true
    },
    new User
    {
        Id = "4",
        ten = "Quốc Huy",
        tuoi = 22,
        gioitinh = "Nam",
        snhat = "2002-02-14",
        hocvan = "Cao đẳng FPT",
        nghenghiep = "Designer",
        thoiquen = "Vẽ, Phim ảnh",
        gthieu = "Thích nghệ thuật và những điều giản đơn.",
        chieucao = 1.72f,
        vitri = "Hà Nội",
        AvatarUrl = "https://randomuser.me/api/portraits/men/4.jpg",
        photos = new List<string> { "https://randomuser.me/api/portraits/men/4.jpg" },
        isVip = false
    },
     new User
    {
        Id = "5",
        ten = "Thảo Nhi",
        tuoi = 21,
        gioitinh = "Nữ",
        snhat = "2003-11-11",
        hocvan = "Học viện Báo chí",
        nghenghiep = "Sinh viên",
        thoiquen = "Nấu ăn, Đọc sách",
        gthieu = "Tìm người cùng đi dạo phố cuối tuần.",
        chieucao = 1.58f,
        vitri = "Hà Nội",
        AvatarUrl = "https://randomuser.me/api/portraits/women/5.jpg",
        photos = new List<string> { "https://randomuser.me/api/portraits/women/5.jpg" },
        isVip = false
    }
        };*/


        private readonly FirestoreDb _db;
public MatchController()
        {
            string path = @"D:\api\key.json";
            Google.Apis.Auth.OAuth2.GoogleCredential credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(path);
            FirestoreDbBuilder builder = new FirestoreDbBuilder
            {
                ProjectId = "login-bb104", // ID Project Firebase của bạn
                Credential = credential
            };

            _db = builder.Build();
        }


        [HttpGet("suggest")]
        public async Task<IActionResult> Suggest(string userId)
        {
            var usersCollection = _db.Collection("Users");
            var snapshot = await usersCollection.GetSnapshotAsync();
            var users = snapshot.Documents
                                .Select(d => d.ConvertTo<User>())
                                .ToList();

            var currentUser = users.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return NotFound(new { message = "User not found" });

           
            var suggestions = users
                .Where(u => u.Id != userId && u.gioitinh != currentUser.gioitinh)
                .ToList();

            return Ok(suggestions);
        }
        [HttpGet("random-suggest")]
        public async Task<IActionResult> GetRandomSuggest(string userId, int limit = 5)
        {
            
            var usersCollection = _db.Collection("Users");
            var snapshot = await usersCollection.GetSnapshotAsync();
            var allUsers = snapshot.Documents.Select(d => d.ConvertTo<User>()).ToList();

           
            var currentUser = allUsers.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) return NotFound("User not found");

            // 3. Lọc & Xáo trộn ngẫu nhiên
            var randomUsers = allUsers
                // Lọc: Khác ID mình và Khác giới tính
                .Where(u => u.Id != userId && u.gioitinh != currentUser.gioitinh)
                // XÁO TRỘN: Sắp xếp theo một mã ngẫu nhiên
                .OrderBy(u => Guid.NewGuid())
                // Lấy số lượng giới hạn (ví dụ 5 người)
                .Take(limit)
                .ToList();

            return Ok(randomUsers);
        }
    }
   


}
