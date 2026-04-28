# Playable Studio 🎬 (Integration Wrapper)

**Playable Studio** là một hệ thống AI Orchestrator chuyên trách nhiệm vụ **Tái chế & Đóng gói Quảng cáo**. Thay vì tự huyễn hoặc code game từ đầu, skill này tiếp nhận một thư mục trò chơi số gốc ĐÃ HOÀN THIỆN, chắt lọc ra tinh túy của game, và tự động sản xuất ra một tệp Playable Ad định dạng `< 5MB`, Single-File HTML5 (Base64) sử dụng Engine Phaser 3.

---

## Tính năng Cốt lõi (Core Features)

1. **Auto-Ingestion & GDD Distillation:** Đọc GDD và Assets List của Game Gốc. Tự động bóc tách và viết lại một bản `Playable_GDD.md` siêu tinh gọn (Tập trung riêng vào khoảnh khắc vàng 15-20s "Hook").
2. **Chốt chặn Ngân sách Dung lượng (Budget Sentinel):** Ép khắt khe giới hạn nguyên liệu đầu vào: Tối đa 1 BGM, 3 SFX và 15 ảnh Sprites để đảm bảo bọc quảng cáo không bao giờ phình to quá mức giới hạn `< 5MB`.
3. **Ủy quyền Sinh Assets Thiếu (Delegated Ordering):** Nếu Playable Ad cần các nút bấm (UI Button, Hand Pointer) mà game gốc không có, hệ thống không báo lỗi. Nó sẽ xuất hóa đơn `Missing_Ad_Assets.json` để bạn gọi skill `game-art-orchestrator` tự vẽ bù.
4. **Boilerplate An Lành (Data-Driven Templates):** Cung cấp sẵn tệp lõi `phaser_base.html`. AI không bao giờ phải hì hục nhớ cách setup framework, loại bỏ 99% rủi ro sinh Code lỗi (Boilerplate Hallucination).
5. **Headless Auto QA:** Tích hợp gọi ngầm `browser_subagent` kích hoạt Node localhost chạy thử 5 giây tự bắt lỗi JavaScript Console ngay trong lúc dev.

---

## Cấu trúc Pipeline (4 Phases)

Tiến trình được chia làm 4 Phase tuân thủ cấu trúc State-Machine cứng:

*   **Phase 1 - INGEST:** AI quét đọc thư mục Game Gốc, phân loại thể loại game (Genre), vẽ ra dòng sự kiện Gameplay Loop, giới hạn chơi, nút thắt Kết Thúc (End Conditions) và bố cục UI Flow hoàn chỉnh. Trả ra `Playable_GDD.md`.
*   **Phase 2 - HARVEST:** Trích xuất nguyên liệu từ Game gốc gộp về thư mục của Ad. Các tệp Ảnh và Âm Thanh sẽ đi qua script tối ưu hóa độ nén xuống `.webp`/`.ogg`. Những tài nguyên phát sinh (chuyên cho Web) được gom vào `Missing_Ad_Assets.json` chờ xử lý nghệ thuật.
*   **Phase 3 - DEV:** Viết khối lượng logic HTML/JS chuyên biệt `.js` theo chuẩn Phaser 3, ánh xạ danh mục các Base64 thông qua `assets_manifest.json`. Máy tự động QA bằng Browser Subagent.
*   **Phase 4 - PACKAGE:** Kích hoạt Python script nhồi toàn khối JavaScript, Sprite Data-URIs và SFX Data-URIs xuyên thẳng vào Template, sinh ra lò file `build/index.html` duy nhất.

---

## Ví dụ Sử dụng (Usage Workflow)

Giả sử bạn đã làm xong một dự án lớn tên là **FlappyTrippy** (có đầy đủ GDD, Hình Ảnh và Âm Thanh gốc).

**Bước 1: Khởi động hệ thống (Kích hoạt Phase 1)**
```text
@[/game-playable-orchestrator] Hãy khởi tạo chiến dịch quảng cáo cho dự án e:\_Project_2026\Assets\Projects\FlappyTrippy\
```
*(AI sẽ đọc GDD gốc, viết lại Playable_GDD.md và hiển thị chờ bạn Appove).*

**Bước 2: Ra lệnh nhảy vọt (Tiếp diễn)**
```text
Tiếp tục
```
*(AI chuyển qua Phase 2: Copy ảnh về kho. Báo cáo dung lượng. Bạn có thể thấy file `Missing_Ad_Assets.json` bật ra, hãy gọi lệnh "@[/game-art-orchestrator] làm cho tao đống đồ này" và nhét nó vào kho).*

**Bước 3 & Bước 4: DEV và Build Thành Phẩm**
```text
Tiếp tục
```
*(Hệ thống sẽ chạy nốt Phase 3 Code Logic và tiếp tục tự chạy Phase 4 đóng gói hoàn tất. Bạn chỉ cần mở thư mục `build/index.html` lên để chiêm ngưỡng sản phẩm).*
