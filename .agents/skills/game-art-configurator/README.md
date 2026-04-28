# Hệ Thống AI Game Artist Pipeline

Tài liệu này cung cấp cái nhìn tổng quan, kiến trúc và hướng dẫn sử dụng chi tiết cho bộ kỹ năng (Agent Skills) tự động hóa quy trình vẽ tài nguyên game bằng AI: **`game-art-compiler`**, **`game-art-orchestrator`** và **`game-art-configurator`**.

---

## 1. Kiến trúc Tổng thể

Quy trình được thiết kế chia làm 3 tầng (Layer) tách biệt, tuân thủ nghiêm ngặt kỹ thuật RAG (Retrieval-Augmented Generation) và Human-In-The-Loop nhằm đảm bảo AI vẽ đúng vật thể và chuẩn xác với Phong cách Nghệ thuật (Art Style) đã định trước.

### A. Tầng Macro: Global Design System
Hệ thống bị chi phối bởi một file thiết kế toàn cục: `Assets/GameArtist/Global_Design_System.json`.
- Đây là bộ luật tối cao quy định Hướng bắt sáng (Lighting), Quy định tỷ lệ khung hình (Dimensions) và Bảng màu UI (Rarity Colors).
- Khi có xung đột xảy ra (bản DNA của một Style yêu cầu vẽ ánh sáng hất từ dưới lên, nhưng Global Rule quy định ánh sáng từ trên xuống), Orchestrator sẽ ép AI phải tuân thủ Global Rule.
- Có thể dùng kỹ năng **`game-art-configurator`** để nhanh chóng cập nhật bảng luật này.

### B. Tầng Micro (Phase 1): Data Ingestion (Chế biến dữ liệu)
Đảm nhiệm bởi skill **`game-art-compiler`**. 
Thay vì bắt AI tự "mò mẫm" style mỗi lần vẽ, bạn chỉ việc cung cấp một thư mục ảnh mẫu. Skill này sẽ tự động:
- Quét toàn bộ ảnh và dùng AI (VLM) để viết caption, phân tích màu sắc, nét vẽ cho từng bức.
- Tự động dùng thư viện `sentence-transformers` chuyển hóa các caption này thành ma trận Vector (`style_index.json`).
- Toán học hóa 2 bức ảnh mang tính "trung bình cộng" làm ảnh đại diện (`Archetype Images`).
- Phân rã luật lệ thành 2 tệp riêng biệt: `Generation_DNA.md` (chuyên dùng để ép AI vẽ) và `Evaluation_Rules.json` (chuyên dùng để soi lỗi).

### C. Tầng Thực thi (Phase 2): Generation & Evaluation (Sinh ảnh & Chấm điểm vòng lặp)
Đảm nhiệm bởi skill **`game-art-orchestrator`**.
Đây là đầu não thực thi khi bạn có nhu cầu vẽ một món đồ mới:
1. **Tìm kiếm (Semantic Search):** Hệ thống đọc file vector để lục kho ảnh gốc tìm ra 2-3 tấm ảnh *giống nhất* với vật thể bạn cấu hình. Nếu độ tương đồng dưới 60%, nó sẽ fallback về dùng ảnh `Archetype`. Đọc file `Global_Design_System.json` để lấy rule chuẩn.
2. **Sinh ảnh (Parallel Gen):** Gọi API tích hợp (Gemini Nanobanana) sinh ra 2 biến thể cùng lúc.
3. **Chấm điểm (Evaluation):** Một đặc vụ (VLM Evaluator) sẽ dùng bộ `Evaluation_Rules.json` và `Global_Design_System.json` để chấm điểm. Ở **Bước 0**, nó kiểm tra nếu vi phạm luật Global (như sai hướng sáng) đánh rớt ngay lập tức. Sau đó mới chấm điểm tổng quan.
4. **Quyết định của Con người (Human-In-The-Loop):** Hệ thống tạm dừng lại, trình bày đánh giá và hỏi ý kiến bạn.
5. **Phiên dịch (Prompt Translation):** Nếu bạn yêu cầu sửa, AI sẽ tự dịch câu chê của bạn thành các tham số kỹ thuật API.

---

## 2. Hướng dẫn Sử dụng (Workflow Thực Tế)

### Bước 0: Khởi tạo/Cập nhật Global Design System (Bất cứ khi nào cần)
Trước khi vẽ bất cứ thứ gì, Hệ thống Thiết kế Toàn cục (`Global_Design_System.json`) sẽ đóng vai trò như Hiến pháp.
1. Nếu bạn cần đổi luật chơi trên quy mô toàn project (Ví dụ: Game của bạn cập nhật từ Top-down xuống Góc nhìn ngang), bạn gõ lệnh:
   > *"Dùng skill game-art-configurator, sửa lại luật Dimension: Bắt buộc nhân vật phải nhìn ngang sang trái."*
2. Hệ thống sẽ tự cập nhật JSON mà bạn không cần mở file code. Mọi thao tác vẽ từ giờ trở đi của mọi Style đều bị bẻ lái theo góc nhìn mới.

### Bước 1: Chuẩn bị Thư viện Style (Chạy 1 lần duy nhất cho mỗi Style)
1. Trong Workspace của bạn, tạo một thư mục chứa ảnh mẫu theo cấu trúc: 
   `Assets/GameArtist/StyleLibrary/DarkFantasy/`
2. Bỏ vào đó 5 - 10 bức ảnh đại diện nét vẽ của Style "DarkFantasy".
3. Gõ lệnh vào Agent: 
   > *"Hãy compile thư mục DarkFantasy"*
4. Skill **`game-art-compiler`** sẽ kích hoạt. Hãy đợi nó quét xong và báo tạo thành công bộ file: `Generation_DNA.md`, `Evaluation_Rules.json` và `style_index.json`.

### Bước 2: Yêu cầu vẽ Asset mới (Chạy thả ga)
Bất cứ khi nào bạn cần vẽ đồ cho game, hãy vứt cho Agent một yêu cầu chuẩn xác.
1. Gõ lệnh vào Agent:
   > *"Sử dụng style DarkFantasy, hãy vẽ cho tôi một bình thuốc độc hình sọ người, phát ra ánh sáng xanh lục"*
2. Kỹ năng **`game-art-orchestrator`** sẽ ngay lập tức được kích hoạt. Bạn sẽ thấy quá trình nó rà soát DNA, nạp ảnh vào prompt và sinh ra 2 Option (Variation A & B).

### Bước 3: Đánh giá & Chỉnh sửa (Vòng lặp)
3. Agent sẽ chặn lại và báo cho bạn biết điểm số VLM chấm cho 2 Option. Ví dụ VLM bảo *Option A bị thiếu ánh sáng xanh lục*.
4. Bạn có 3 lựa chọn:
   - **(Approve):** "Lấy Option B". (Quy trình kết thúc).
   - **(Auto Redo):** "Chạy lại vòng 2 dùng nguyên feedback của VLM đi".
   - **(Manual Override):** "Chạy vòng 2, nhớ ép bình thuốc lùn xuống, và cái đầu sọ phải nứt nẻ".
5. Hệ thống phi thẳng sang Round 2 với Translation Parameters và nôn ra thành phẩm tối ưu nhất!

---

## 3. Ví Dụ Minh Họa Cụ Thể (Example)

**Hội thoại giả lập giữa User và Antigravity Agent:**

**User:** 
> Hãy compile thư mục `PixelArt_Cyberpunk` nhé.

**Agent:** 
> Khởi động bộ Ingestion Pipeline... 
> [Logging] Đã quét 8 ảnh.
> Xong! Thư mục `PixelArt_Cyberpunk` đã được Index. Hệ Rules đã sẵn sàng!

**(Chỉnh sửa Global Design System)**

**User:** 
> Gần đây Game đổi style bóng đổ. Dùng `game-art-configurator` sửa cái Global Lighting thành: "Ánh sáng đánh ngược từ dưới lên trên" giúp mình.

**Agent:**  
> Đã cập nhật `Global_Design_System.json`: `"global_lighting": "Ánh sáng đánh ngược từ dưới lên trên"`. Luật mới sẽ áp dụng ngay lập tức cho các lần sinh ảnh tiếp theo của toàn bộ project.

**(1 tuần sau)**

**User:** 
> Style PixelArt_Cyberpunk, vẽ 1 khẩu súng ngắn tự động.

**Agent:** 
> Xác nhận sử dụng style `PixelArt_Cyberpunk`. 
> [Logging] Quét Semantic Search: độ tương đồng tìm thấy file `sniper.png` và `laser_pistol.png` > 80%. Sử dụng 2 ảnh làm Style Reference.
> Đã sinh xong 2 biến thể súng ngắn (A và B).
> 
> **🖨️ Evaluator VLM Report:**
> - Variant A: Checklist [Pass], Aesthetic: 92/100.
> - Variant B: Checklist [Fail] (Súng dính viền outline mờ - vi phạm lệnh cấm trong JSON), Aesthetic: 45/100.
> 
> Chào bạn, đây là 2 biến thể Variant A và B. Bạn muốn chốt luôn Variant A, hay muốn mình re-roll Variant B với lỗi Outline mờ, hay bạn muốn bổ sung thêm điều kiện gì không?

**User:** 
> Lấy Variant A, nhưng màu thép ở báng súng bóng bẩy hơn tí. Chạy lại vòng 2 cho nó bằng Guideline đó.

**Agent:**  
> Đang Dịch Prompt... *Cập nhật Positive Rule `(glossy metal stock:1.4)`*. Sinh ảnh Round 2...
> Hoàn tất! Dưới đây là thành phẩm cuối cùng!
