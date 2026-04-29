# Hướng Dẫn Sử Dụng: Game Designer Skill

## Tổng quan
Skill **`game-designer`** đóng vai trò là một Master Orchestrator (Người Điều Phối Đỉnh Cao) trong quy trình thiết kế và sản xuất Game. Chuyên chịu trách nhiệm tiếp nhận ý tưởng sơ khai của bạn và nhào nặn nó thành một hệ thống tài liệu **Game Design Document (GDD)** tiêu chuẩn cùng các Checklist sản xuất siêu chi tiết.

Với kiến trúc nâng cấp mới nhất, `game-designer` hoạt động dưới dạng một **Autonomous Pipeline 5 Bước (5-Phase Pipeline)**. Quá trình này giúp loại bỏ hoàn toàn việc "tràn bộ nhớ" của AI bằng cách sinh ra tuần tự các tài liệu thiết kế chuyên biệt, tự động rà soát lỗi chéo (Cross-QA) giữa các phòng ban, và đặc biệt là khả năng **tự động vẽ Wireframe giao diện (UI)** dựa trên văn bản.

## Cách sử dụng cơ bản

Trong khung chat với AI, bạn chỉ cần gọi tên skill kèm theo ý tưởng hạt giống:

```text
@/game-designer [Ý_TƯỞNG_GAME_HOẶC_LINK_THAM_KHẢO]
```

---

## Các Ví dụ Tham khảo (Example Prompts)

Tùy vào hoàn cảnh, bạn có thể sử dụng Skill này theo nhiều dạng Input khác nhau:

### Dạng 1: Đưa ra ý tưởng sơ khai (Raw Idea)
Nếu bạn có một ý tưởng nổ ra ngẫu nhiên trong đầu, cứ việc ném vào Skill.
> `@/game-designer Tôi muốn làm một game hypercasual đua tự do. Nhân vật là một viên kẹo dẻo chạy trên đường dây điện, vuốt sang trái phải để nhảy né chim chóc. Mục đích chỉ để leo rank điểm cao nhất.`

### Dạng 2: Tạo Clone/Tách kỹ năng từ game có sẵn (References)
Dùng khi bạn muốn tham khảo một game trên thị trường hoặc từ 1 video tiktok gameplay.
> `@/game-designer Hãy phân tích core loop của trò "Phone Case DIY". Lược qua các tính năng trang trí rườm rà, hãy viết một bản GDD Prototype MVP cốt lõi tập trung hoàn toàn vào kỹ năng "Xịt Sơn" lên ốp lưng để DEV team có thể test độ mượt của vật lý sơn.`

### Dạng 3: Dò trend và Tự động xây dựng (Trend Research)
Nhờ AI tự làm game dựa trên xu hướng thị trường.
> `@/game-designer Hãy duyệt nhanh thị trường xem thể loại giải đố ASMR phân loại ốc vít (Wood Nuts & Bolts) đang có luật chơi cụ thể ra sao. Từ đó dựng ra bản cấu trúc GDD chi tiết cho một game clone tương tự nhưng lấy theme là Sắp Xếp Hành Lý Sân Bay.`

---

## Luồng hoạt động tự động của AI (5-Phase Pipeline)
Khi bấm gửi câu lệnh, AI sẽ kích hoạt chuỗi tiến trình sản xuất hoàn toàn tự động, chỉ dừng lại duy nhất 1 lần để xin xác nhận từ bạn.

### 1. Phase 1: GDD Master Generation (Điểm dừng xin duyệt)
- Phân tích cốt lõi (Coreloop, Mechanics, USP).
- Xuất ra file GDD Tổng quan `[ProjectName]_Master_GDD.md`.
- **TẠM DỪNG:** AI sẽ dừng lại và hỏi bạn có đồng ý với bản thiết kế này không. Nếu bạn "Duyệt", hệ thống sẽ tự động chạy 1 mạch 4 bước còn lại mà không cần sự can thiệp của bạn.

### 2. Phase 2: Department Breakout (Bóc tách phòng ban)
- Đọc GDD Tổng quan và bóc tách thành 4 tài liệu yêu cầu (Requirements) riêng biệt cho từng khối, xuất thành file Markdown độc lập:
  - `[ProjectName]_Art_Requirements.md` (Danh sách Asset 2D/3D cần thiết)
  - `[ProjectName]_Dev_Architecture.md` (Luồng Logic Code, Cấu trúc Game Manager)
  - `[ProjectName]_AudioVFX_List.md` (Danh sách SFX, BGM, & Hiệu ứng hình ảnh)
  - `[ProjectName]_UI_Plan.md` (Quy hoạch cấu trúc toàn bộ các màn hình UI)

### 3. Phase 3: UI Wireframing (Sinh bản vẽ UI trực quan)
- AI tự động đọc file `UI_Plan` vừa tạo.
- Đối với mỗi màn hình/Popup được định nghĩa, AI sẽ tự động gọi công cụ sinh ảnh để vẽ bản nháp **Wireframe Layout** (Đen trắng, phẳng, chú trọng vị trí UX) và lưu trữ thẳng vào thư mục của bạn.

### 4. Phase 4: Cross-Validation & Auto-Correction (Tự động QA rà soát chéo)
- AI sẽ nạp toàn bộ 4 file Breakout lên để phân tích tính đồng bộ. 
- *Ví dụ:* Nếu luồng Code có sự kiện "Người chơi bị dính sát thương", nhưng bảng Âm thanh lại quên liệt kê SFX tiếng kêu bị thương, AI sẽ tự động hiểu ra lỗi logic này và chèn bù tài sản đó vào file `AudioVFX_List`.

### 5. Phase 5: Integration Map (Bản đồ Tích hợp)
- AI tạo ra file cuối cùng `[ProjectName]_Integration_Map.md` để kết nối tất cả các thành phần lại với nhau.
- File này sẽ mô tả các "Điểm neo" (Hook) rõ ràng: **Trigger Event** nào -> **Chạy Code** gì -> Bật **UI** nào -> Kích hoạt **VFX/SFX** nào, giúp Lập trình viên dễ dàng ráp tài nguyên.

Khi 5 Phase kết thúc, bạn sẽ nhận được một thư mục dự án hoàn chỉnh tài liệu để trao ngay cho team Production tiến hành Code & Vẽ!
