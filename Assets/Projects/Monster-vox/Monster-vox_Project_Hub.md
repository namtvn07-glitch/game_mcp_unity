# Monster-vox Project Hub (Điều Phối & Hướng Dẫn)

Chào mừng đến với dự án **Monster-vox (Spooky Beats)**! 

File này đóng vai trò là "Trạm trung chuyển" (Hub) giúp tất cả các thành viên trong dự án dễ dàng nắm bắt tổng quan, tìm kiếm đúng tài liệu thuộc chuyên môn của mình và hiểu cách thức kết nối với các bộ phận khác.

---

## 1. TỔNG QUAN DỰ ÁN (PROJECT OVERVIEW)
- **Tên dự án:** Monster-vox
- **Thể loại:** Casual / Music Sandbox
- **Core Gameplay:** Biến tiếng ồn/giọng nói thu từ micro thành bản mix âm nhạc nhịp điệu. Kéo thả âm thanh vào quái vật để tạo beat.
- **Phong cách:** "Spooky-Derpy" (Ma mị nhưng ngớ ngẩn, đáng yêu).
- **Mục tiêu:** Tạo trải nghiệm thỏa mãn tức thì (Instant Gratification) chỉ trong 3 giây thu âm, tối ưu cho nội dung viral trên TikTok/Shorts.
- **Tài liệu gốc:** [Monster-vox_Master_GDD.md](./Monster-vox_Master_GDD.md)

---

## 2. HƯỚNG DẪN TÌM TÀI LIỆU THEO BỘ PHẬN (DEPARTMENT DIRECTORY)

Để tránh nhiễu loạn thông tin, mỗi bộ phận chỉ cần tập trung vào các tài liệu được liệt kê dưới đây.

### 🎨 Art & Animation Team (Đồ họa & Hoạt ảnh)
**Nhiệm vụ:** Thiết kế quái vật "Spooky-Derpy", vẽ UI 2D (9-slice), tạo sprite sheet.
- **Tài liệu yêu cầu Art:** [Monster-vox_Art_Requirements.md](./Monster-vox_Art_Requirements.md)
  *(Chứa danh sách tất cả asset cần vẽ: Monster, Background, Prop, Icon, UI assets)*
- **Kiểm tra UI Layout:** [Monster-vox_UX_Wireframes.md](./Monster-vox_UX_Wireframes.md)

### 💻 Development Team (Lập trình viên)
**Nhiệm vụ:** Xây dựng Core Loop, xử lý âm thanh Microphone & Quantization, quản lý state (Manager), tích hợp toàn bộ game.
- **Kiến trúc hệ thống (Bắt buộc đọc):** [Monster-vox_Dev_Architecture.md](./Monster-vox_Dev_Architecture.md)
  *(Sử dụng Singleton Managers, Object Pooling, Event-Driven UI)*
- **Yêu cầu tính năng:** [Monster-vox_Dev_Requirements.md](./Monster-vox_Dev_Requirements.md)
- **Sơ đồ tích hợp chéo (Cực kỳ quan trọng):** [Monster-vox_Integration_Map.md](./Monster-vox_Integration_Map.md)
  *(Chứa các System Hooks để bắn event cho UI, Audio, VFX và Art)*

### 🔊 Audio & VFX Team (Âm thanh & Hiệu ứng)
**Nhiệm vụ:** Sáng tác BGM nhịp điệu mạnh, làm SFX tương tác, thiết kế Particle System (VFX).
- **Danh sách Âm thanh & VFX:** [Monster-vox_AudioVFX_List.md](./Monster-vox_AudioVFX_List.md)
  *(Chứa danh sách cụ thể từng BGM, SFX, Voice Filter và các Particle Effects cần thiết)*

### 📱 UI/UX Team (Giao diện người dùng)
**Nhiệm vụ:** Lên luồng UX, thiết kế bố cục màn hình (Portrait), hướng dẫn tích hợp UI vào Unity.
- **Sơ đồ tư duy & Luồng UX:** [Monster-vox_UX_Wireframes.md](./Monster-vox_UX_Wireframes.md)
  *(Xem sơ đồ Mermaid và ASCII wireframes cho Main Menu, Stage, Store)*
- **Kế hoạch UI:** [Monster-vox_UI_Plan.md](./Monster-vox_UI_Plan.md)
- **Hướng dẫn tích hợp UI cho Dev:** [Monster-vox_UI_Integration_Guide.md](./Monster-vox_UI_Integration_Guide.md)

### 📊 Game Design & Economy (Thiết kế game & Dữ liệu)
**Nhiệm vụ:** Cân bằng kinh tế (Spooky Coin), thiết lập giá cả IAP/Ads, số lượng nội dung.
- **Bảng dữ liệu & Cân bằng:** [Monster-vox_Game_Data.md](./Monster-vox_Game_Data.md)
  *(Chứa thông số giá cả unlock Monster, Theme, Slot và drop rate)*

---

## 3. QUY TRÌNH TÍCH HỢP (INTEGRATION WORKFLOW)

Khi các tài nguyên (Asset/Code) đã hoàn thành độc lập, team Development sẽ làm đầu mối để ghép nối (Integrate) mọi thứ lại với nhau.

**Các bước ghép nối chuẩn:**
1. **Dev** tạo Scene và Hierarchy chuẩn theo `Dev_Architecture.md`.
2. **Art** export Sprite và UI đẩy vào thư mục `GameAssets/`.
3. **Audio** export `.wav/.ogg` đẩy vào thư mục `SFX/`.
4. **Dev** kết nối các Event dựa trên **[Monster-vox_Integration_Map.md](./Monster-vox_Integration_Map.md)**. Bất cứ khi nào một sự kiện xảy ra (VD: Kéo thả bong bóng thành công), Dev sẽ nhìn vào Integration Map để biết phải gọi hiệu ứng VFX nào, phát SFX nào, và đổi animation Art nào.

> **💡 Lời khuyên:** Nếu có sự mâu thuẫn giữa các file, hãy lấy **[Master GDD](./Monster-vox_Master_GDD.md)** và **[Integration Map](./Monster-vox_Integration_Map.md)** làm chuẩn cuối cùng. Chúc cả team làm việc hiệu quả!
