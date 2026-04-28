# Game Art Orchestrator

## Overview
The `game-art-orchestrator` là hạt nhân điều phối cuối cùng trong pipeline thiết kế Game Art tự động. Nó đóng vai trò như một **AI Art Director & Visual Validator**, tự động hóa toàn bộ quá trình biến text prompt của người dùng thành một tài sản game (Game Asset) hoàn chỉnh, thông qua việc ứng dụng công nghệ RAG nhúng (Embedding), kiểm thử VLM và kỹ thuật Few-Shot Prompting.

Phần lõi của kỹ năng này đã được nâng cấp mạnh mẽ để có thể tự động tính toán Cosine Similarity xác suất cao thông qua tập lệnh Python thay vì quy trình phỏng đoán thụ động của mô hình ngôn ngữ (LLM).

---

## ⚙️ Cấu trúc luồng thực thi (The Pipeline)

Quá trình cấp số nhân chất lượng hình ảnh được chia làm 3 Phase khép kín:

### Phase 1: Retrieve & Prompt Engineering (RAG & Few-Shot)
Giải quyết triệt để vấn đề *"Ảo giác phong cách"* (Style Hallucinations) thông qua bộ lọc thông tin toán học.

1. **RAG Context Retrieval**: Orchestrator kích hoạt tập lệnh nội bộ `scripts/retrieve_orchestrator_context.py`.
2. Môi trường Vector (Sentence-Transformers) tự động phân tích prompt gốc để đưa ra so khớp toán học:
   - Truy vấn `Assets/GameArtist/global_index.json` để nhúng 3 quy tắc nền tảng chặt chẽ nhất.
   - Quét qua `style_index.json` của phong cách gốc, tính toán **Cosine Similarity > 0.60** để chọn hình ảnh Reference (mấu chốt). Nếu không có ảnh nào phù hợp, hệ thống tự động Fallback xuống các `Archetypes` trụ cột.
3. LLM Agent tiến hành kết hợp dữ liệu trả về với file `Generation_DNA.md` làm nền tảng nén Prompt chuẩn mực nhất.

### Phase 2: Generation & Interactive Self-Correction Loop
Luân chuyển giữa "Người Sáng Tạo" và "Nhà Phê Bình".

1. **Double Production**: Hệ thống sử dụng Tool Gen-AI liên kết để trả về 2 biến thể riêng biệt phòng rủi ro.
2. **VLM Self-Evaluation**: Agent hóa thân thành chuyên gia thẩm định (Reviewer) đọc chéo 2 bức ảnh dựa trên File `Evaluation_Rules.json` của hệ thống.
3. **Human-In-The-Loop (HITL)**: Workflow sẽ dừng lại tại Checkpoint này và hỏi ý kiến con người:
   - Bạn được phép phê duyệt (Approve) 1 trong 2 biến thể.
   - Hoặc cung cấp thêm từ khóa, nhận xét để Agent tự động thực hiện **Prompt Translation** (Mã hóa review của bạn thành từ khóa kỹ thuật) và chạy tiếp Round 2.

### Phase 3: Asset Finalization & Export
Tự động hóa hệ thống File Management giống hệt một studio chuyên nghiệp.

1. Asset sau khi phê duyệt sẽ tự động di chuyển về thư mục tương ứng `Assets/GameArtist/Generated/<Style_Name>/<Category>/`.
2. Tự động chuyển đổi Tên hóa (Nomenclature: `[object]_[YYYY-MM-DD].png`).
3. Ghi log kiểm kê vào tệp báo cáo `Generated_Asset_Catalog.md` trên mạng lưới gốc và bàn giao link tuyệt đối.

---

## 🛠 Script Core Utilities

- **`scripts/retrieve_orchestrator_context.py`**:
  Engine RAG cốt lõi lấy data dựa trên Vector Distance để nhồi vào prompt tạo ảnh. Script này cho phép LLM tiết kiệm hàng loạt token và luôn bám dính vào luật cốt lõi nhất.
- **`scripts/downscale_image.py`**:
  Tiện ích tối ưu hóa ảnh đầu ra khi sử dụng những Model Vision giới hạn Token tránh lỗi token-overflow hoặc để nén ảnh UI có độ nét quá lớn.

---

## 📂 Các dữ liệu đầu vào (Consumed Contexts)
Để Orchestrator này hoạt động, nó phụ thuộc chặt chẽ vào output của `game-art-compiler` và `game-art-configurator`:

* `Assets/GameArtist/global_index.json`: Vector lưới hệ thống vật lý và UI chung.
* `Assets/GameArtist/StyleLibrary/<style_name>/Generation_DNA.md`: Văn bản DNA mỹ thuật.
* `Assets/GameArtist/StyleLibrary/<style_name>/style_index.json`: Vector lưới định dạng Few-shot/Archetype từ folder kho ảnh gốc.
* `Assets/GameArtist/StyleLibrary/<style_name>/Evaluation_Rules.json`: Thang điểm ma trận chấm lỗi VLM.
