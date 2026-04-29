# Monster-vox Master GDD

### 1. [CONCEPT & AUDIENCE]
- **Project Name:** Monster-vox (Spooky Beats)
- **Genre:** Casual / Music Sandbox
- **Target Audience:** Người chơi Casual trên nền tảng di động (iOS/Android), tập trung vào tệp người dùng thích sáng tạo nội dung nhanh trên TikTok/Shorts.
- **USP (Unique Selling Proposition):** Biến mọi tiếng ồn thu từ micro thành một bản mix âm nhạc nhịp điệu hoàn chỉnh, tự động cắt và ép nhịp (Auto-Quantize), với hình ảnh quái vật "Spooky-Derpy" độc đáo và tính năng chia sẻ viral tích hợp.

### 2. [CORE_LOOP & GAMEPLAY]
Vòng lặp cốt lõi tập trung vào sự thỏa mãn tức thì (Instant Gratification):
- **Core Loop:** Người chơi chọn Theme (Sảnh chính) -> Vào Sân khấu, thu âm thanh ngẫu nhiên (<2s) -> Kéo thả bong bóng âm thanh vào Quái vật để tạo bản mix -> Quái vật tự động nhún nhảy và hát theo nhịp -> Thu thập Tiền rơi ra -> Dùng tiền nâng cấp/mở khóa Theme và Quái vật mới.
- **Mechanics:** 
  - Chạm và giữ (Hold) nút đỏ để thu âm (<2 giây).
  - Kéo và thả (Drag & Drop) bong bóng âm thanh vào các slot (bục đứng của quái vật).
  - Tap để thu thập xu rơi trên màn hình.
- **Pacing & Atmosphere:** 
  - Nhịp độ nhanh, tạo tiếng cười và dopamine ngay lập tức chỉ sau 3 giây thu âm. 
  - Phong cách "Spooky-Derpy": Đồ hoạ 2D hơi méo mó, màu sắc bắt mắt nhưng tông tối (tím, đen, xanh neon). Quái vật rùng rợn nhưng có biểu cảm ngớ ngẩn. Âm thanh nhạc nền (BGM) sắc nét, nhịp beat mạnh mẽ, có nhiều khoảng trống tần số để tôn lên giọng người chơi.
- **Camera Behavior:** Camera tĩnh (Fixed Camera), góc nhìn trực diện, màn hình dọc (Portrait) tối ưu cho thao tác một tay và quay video TikTok.

### 3. [FEATURES LIST]
Danh sách tính năng cho MVP Prototype:
- **Hệ thống Sảnh chính (Home/Theme Selection):** Menu dạng Playlist hoặc cánh cửa để chọn Stage.
- **Hệ thống Thu âm & Xử lý Âm thanh:** Ghi âm microphone (<2s) và thuật toán Auto-Quantize (ép nhịp BPM), kết hợp Voice Filters (Pitch-up, trầm, xoắn giọng).
- **Cơ chế Mixing (Kéo & Thả):** Hệ thống Drag & Drop gán âm thanh vào Monster Slots (Bắt đầu với 3 Slot mặc định).
- **Hệ thống Kinh tế & Nâng cấp (Meta Progression):** Cơ chế sinh xu dựa trên chu kỳ phát nhạc hoàn chỉnh, dùng để mở khóa Theme, Monster và Slot biểu diễn.
- **Hệ thống Chia sẻ Viral:** Nút In-game Screen Recording để xuất video.
- **Hệ thống Kiếm tiền (Monetization):** Tích hợp Rewarded Ads (nhận xu, mở khóa nhanh VIP, thêm slot) và IAP (Premium Access, Coin Packs).

### 4. [TECHNICAL_CONSTRAINTS]
- **Platform/Engine:** Mobile (iOS/Android) phát triển bằng Unity Engine.
- **Orientation:** Portrait (Màn hình dọc).
- **Target Performance:** Tối ưu hóa bộ nhớ cho nhiều bộ đệm âm thanh (Audio Buffers). Đảm bảo độ trễ âm thanh bằng không (Zero latency) và ép nhịp chính xác (Beat-snapping) tuyệt đối. Mượt mà ở 60 FPS.

### 5. [ECONOMY & GAME DATA]
- **Currencies:** 
  - **Soft Currency:** Spooky Coin (Tiền vàng) - Đơn vị tiền tệ chính trong game.
  - **Hard Currency:** Không sử dụng Hard Currency trong MVP, người chơi nạp tiền thật (IAP) sẽ quy đổi trực tiếp ra Spooky Coin hoặc mua Premium Access.
- **Sources & Sinks:**
  - **Sources (Nguồn sinh):** Rớt tự động từ quái vật (ví dụ: mỗi khi bản nhạc chạy hoàn thành 1 chu kỳ loop), xem quảng cáo Rewarded Ads để nhận lượng lớn xu, Mua gói xu qua IAP.
  - **Sinks (Nguồn tiêu):** Tiêu xu (hoặc xem Ads) để mở khóa Theme mới (bài hát & bối cảnh mới), mở khóa Monster mới (ngoại hình & bộ lọc giọng hát mới), mở rộng Slot biểu diễn (tăng từ 3 lên tối đa 5 slot trên sân khấu).
- **Core Entities Quantity:**
  - **Themes:** 10 Themes đa dạng bối cảnh.
  - **Monsters:** 20 loại quái vật với 20 bộ Voice Filters độc đáo.
  - **Slots:** Mặc định 3 Slots trên sân khấu, nâng cấp tối đa lên 5 Slots.
