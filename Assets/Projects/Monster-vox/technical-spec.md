# BIÊN BẢN ĐẶC TẢ YÊU CẦU KỸ THUẬT (TECHNICAL SPECIFICATION)
**Dự án:** Monster Vox (Prototype V1.0)
**Module:** Hệ thống Ghi âm (Record) & Tự động căn nhịp (Auto-Quantize)
**Người tạo:** Game Designer
**Người nhận:** Development Team

## 1. Mục Đích Tài Liệu
Đặc tả logic hoạt động của hệ thống thu âm giọng nói người dùng và thuật toán tự động đồng bộ (sync) đoạn âm thanh đó vào nhịp điệu (BPM) của nhạc nền có sẵn. Yêu cầu ưu tiên số 1: **Tuyệt đối không có độ trễ (Zero Latency) khi phát âm thanh.**

## 2. Các Hằng Số Hệ Thống (Master Constants)
Dev cần thiết lập các biến số này ở mức Global/Singleton để toàn bộ hệ thống tham chiếu:

| Tên Biến | Giá trị mặc định | Giải thích logic |
| :--- | :--- | :--- |
| `MASTER_BPM` | 120 | Tốc độ nhịp nền của game (Beats Per Minute). |
| `MAX_RECORD_TIME` | 2000 ms | Thời gian thu âm tối đa cho phép (bằng đúng 4 nhịp). |
| `NOISE_GATE_THRESHOLD` | -40 dB | Ngưỡng âm lượng tối thiểu. Dưới mức này coi là khoảng lặng. |
| `SNAP_GRID_INTERVAL` | 500 ms | Mốc nhịp (1/4 note ở 120 BPM). Âm thanh chỉ được phép bắt đầu phát tại các mốc thời gian chia hết cho 500ms. |

## 3. Đặc Tả Logic Luồng Xử Lý (Logic Flow Specification)

### 3.1. Tính Năng Ghi Âm (Audio Input Module)
* **Trigger Start:** `OnPointerDown` vào nút [RECORD].
    * Hệ thống bắt đầu ghi dữ liệu từ Microphone vào buffer.
* **Trigger Stop:** Kích hoạt khi xảy ra 1 trong 2 điều kiện:
    1.  `OnPointerUp` (Người dùng nhả nút).
    2.  Bộ đếm thời gian chạm mốc `MAX_RECORD_TIME` (2000ms).
* **Output:** Tạo ra một `Raw Audio Data` tạm thời trong RAM.

### 3.2. Tính Năng Tiền Xử Lý (Pre-processing & Trimming Module)
*Ngay sau khi có `Raw Audio Data`, hệ thống chạy ngầm thuật toán dọn dẹp trước khi cho phép người chơi kéo thả.*
* **Thuật toán Cắt khoảng lặng (Silence Trimming):**
    * **Bước 1:** Quét mảng dữ liệu âm thanh từ Sample đầu tiên (Index 0) tiến lên. Bỏ qua tất cả các Sample có biên độ (Amplitude) `< NOISE_GATE_THRESHOLD`. Tìm được Sample hợp lệ đầu tiên -> Cắt mốc `StartPoint`.
    * **Bước 2:** Quét ngược từ Sample cuối cùng lùi lại. Bỏ qua các Sample `< NOISE_GATE_THRESHOLD`. Tìm được Sample hợp lệ cuối cùng -> Cắt mốc `EndPoint`.
* **Output:** Trả về một `Trimmed AudioClip` (File âm thanh sạch) và chuyển sang trạng thái sẵn sàng để người chơi kéo thả (Drag & Drop).

### 3.3. Tính Năng Đồng Bộ Nhịp (Auto-Quantize & Beat Snapping Module)
*Đây là thuật toán quan trọng nhất. Yêu cầu sử dụng đồng hồ âm thanh của Engine (Ví dụ: `AudioSettings.dspTime` trong Unity), KHÔNG dùng `Update()` hay `Time.deltaTime`.*

* **Logic Master Clock:** Nhạc nền (Base Beat) bắt đầu chạy tại thời điểm `T = 0`. Các mốc nhịp (Grid) xảy ra liên tục tại `T = 500ms, 1000ms, 1500ms...`
* **Logic Snap (Khớp nhịp):**
    * Khi người chơi thả (Drop) `Trimmed AudioClip` vào một Quái vật trên màn hình tại thời điểm thực tế là `t_drop` (ví dụ: `t_drop = 1200ms`).
    * Hệ thống **không được** phát (Play) AudioClip ngay lập tức.
    * Hệ thống tính toán mốc `SNAP_GRID_INTERVAL` gần nhất tiếp theo (trong trường hợp này là `1500ms`).
    * Lên lịch phát AudioClip (Schedule Play): Cài đặt độ trễ `Delay = 1500ms - 1200ms = 300ms`.
* **Logic Looping (Lặp lại vòng lặp):**
    * Đọc độ dài (Duration) của `Trimmed AudioClip` (ví dụ: độ dài file là `700ms`).
    * Tìm mốc Grid chẵn lớn hơn độ dài file: `700ms < 1000ms` (2 lưới).
    * Hệ thống tự động cài đặt chu kỳ lặp (Loop Interval) cho Quái vật này là `1000ms`. (Tức là nó sẽ phát 700ms, sau đó im lặng 300ms, rồi lặp lại).

## 4. Yêu Cầu Hiệu Năng & Tối Ưu (Non-Functional Requirements)
1.  **Memory Management:** Mỗi lần người dùng nhấn nút Record, hệ thống phải giải phóng (Dispose/Destroy) bản ghi cũ khỏi RAM trước khi tạo bản ghi mới để tránh tràn bộ nhớ (Memory Leak).
2.  **Audio Mixer:** Tất cả các âm thanh thu âm phải được định tuyến qua một kênh Mixer chung để có thể tùy chỉnh Volume tổng (tránh trường hợp người chơi thu tiếng quá to lấn át nhạc nền).

## 5. Tiêu Chí Nghiệm Thu (Definition of Done - Checklist Test)
Dev bàn giao bản build cho Design pass các trường hợp test sau:
* [ ] Bấm giữ nút dưới 2 giây và nhả ra: Cắt file âm thanh chính xác.
* [ ] Bấm giữ nút quá 2 giây: Tự động ngắt thu và lưu file ngay lập tức.
* [ ] Kéo thả 1 đoạn thu âm tiếng "Ah" vào game: Tiếng "Ah" luôn lặp lại đúng vào nhịp tiếng trống (kick) của nhạc nền, không bị lệch nhịp.
* [ ] Kéo thả 3 đoạn thu âm khác nhau vào màn hình cùng lúc: Cả 3 âm thanh lặp lại đồng bộ với nhau tạo thành một giai điệu, không phát lộn xộn.