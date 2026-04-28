# 🎮 Monster Vox (Spooky Beats)

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-blue.svg)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android-lightgrey.svg)]()
[![Genre](https://img.shields.io/badge/Genre-Casual--Music-orange.svg)]()

> **"Biến mọi tiếng ồn ngớ ngẩn của bạn thành một bản hit kinh dị - Dễ dàng, Gây nghiện và Viral."**

**Monster Vox** là một tựa game Casual thể loại âm nhạc sáng tạo (Music Sandbox) dành cho nền tảng Mobile (màn hình dọc). Khai tử hoàn toàn các hệ thống Idle phức tạp, game đưa người chơi bước thẳng vào tính giải trí thuần tuý.

Hãy tưởng tượng sự thỏa mãn khi phối nhạc của *Incredibox (Sprunki)* kết hợp với khả năng tấu hài nhại giọng của *Talking Tom*. Đây là nơi người chơi tự tạo ra một ban nhạc quái vật bằng chính giọng nói của mình, mở khoá các không gian Sân khấu đa dạng và tạo ra các bản phối điên rồ!

---

## ✨ Tính Năng Nổi Bật (Features)

- **🎙️ UGC "Siêu Lười" (Effortless UGC):** Thu một tiếng hắt xì, chó sủa hay một câu nói đùa vào micro, game sẽ biến nó thành âm nhạc.
- **🎛️ Phép Màu Auto-Quantize:** Thuật toán tự động cắt, kéo giãn và ép mọi âm thanh rớt vào đúng nhịp (BPM) bài nhạc nền gốc.
- **🎨 Môi Trường Đa Dạng (Themes):** Hệ thống màn hình Home đưa bạn đến các bối cảnh Sân khấu khác nhau, mỗi màn đi kèm một Bản nhạc nền (BGM) riêng biệt cực kỳ bắt tai.
- **🛍️ Hệ Thống Tiền Tệ Tối Giản:** Tự động rơi xu (Farm dạo ngẫu nhiên) khi biểu diễn bản mix. Tiền thu được dùng để mua thêm "Chỗ Đứng" (Slots) cho quái vật, Quái vật ngoại hình mới, hoặc Mở Khoá Theme nhạc mới!
- **📱 Chuyên Trị Viral:** Game tích hợp nút Screenshot / Record Screen dễ dàng chia sẻ bản mix ngớ ngẩn lên TikTok, Shorts.

---

## 🕹️ Vòng Lặp Trò Chơi (Core Loop)

Trải nghiệm diễn ra mượt mà và tập trung:

### 1. Home / Theme Selection
- Người chơi bắt đầu tại Home. Chọn một Sân khấu (Theme) mà mình đã mở khoá để vào quẩy.
- Thấy các Theme/Nhạc nền cao cấp bị khóa, tạo động lực để chơi cày tiền.

### 2. The "Fun" Phase (Trên Sân Khấu)
- **Thu âm:** Giữ nút [RECORD] để thu bất kỳ tiếng động nào (dưới 2 giây).
- **Kéo & Thả:** Ném bong bóng âm thanh vào Quái vật chờ sẵn trên các bục (Slots).
- **Phát nhạc:** Quái vật hòa giọng cùng nhịp điệu (Theme BGM) tạo thành bản mix điên cuồng.

### 3. Khai Thác & Mở Khoá (Meta Progression)
- Quái vật hò hét liên tục sẽ sinh ra các phần thưởng (Xu) ngay trên màn hình.
- Rút xu về Home -> Chi tiêu: Mở khoá bài hát mới (Theme), mở khoá quái vật mới, hoặc thêm bục (từ 3 lên 4 hoặc 5 con quái vật 1 lúc) để chơi lớn!

---

## 🛠️ Yêu cầu kỹ thuật (Requirements)

- **Unity Engine:** Phiên bản `6000.x` (Hoặc tối thiểu `2022.3 LTS`)
- **Nền tảng mục tiêu:** iOS 12+ / Android 8.0+
- Yêu cầu cấp quyền sử dụng **Microphone** trên thiết bị để hoạt động hệ thống đo nhịp và thu âm Auto-Quantize.

## 🚀 Hướng Dẫn Chạy Demo (Prototype)

1. Mở Unity Hub và nạp thư mục dự án `Unity-MCP`.
2. Mở Scene chính tại: `Assets/Projects/Monster-vox/Scenes/demo.unity`
3. Nhấn **Play**.
4. Chọn quái vật, click **Record** và nói gì đó vào mic. 
5. *(Nếu không có Micro)* Nhấn **Space** trong Editor để test chế độ Spawn Dummy Clip ngay lập tức (kéo thả không cần thu âm).
6. Kéo âm thanh vào một quái vật và thưởng thức!
