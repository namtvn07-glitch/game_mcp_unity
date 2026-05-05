# Monster-vox Dev Requirements

*(Lưu ý: Tài liệu này định nghĩa logic và luật chơi. Developers tự quyết định architecture, singletons, thư viện và tên script).*

## 1. Core Logic Rules & Flow
- **Game States:** Game chỉ cần 2 trạng thái chính: `HomeState` và `StageState`.
- **Luồng dữ liệu thu âm (Microphone):** 
  - Khi gán Monster vào Slot, hiển thị `NewAnimalPopupUI`.
  - Nhấn nút Record trên popup (hoặc đợi đếm ngược tối đa 2s), thiết bị bắt đầu lưu data vào Audio Buffer. Trong quá trình thu âm, BGM sẽ bị tắt (mute).
  - Khi thả nút hoặc hết thời gian đếm ngược, buffer này phải được phân tích: Cắt bỏ khoảng lặng (Trim Silence) ở đầu và cuối.
  - Sau đó, hệ thống giữ nguyên độ dài AudioClip gốc nhưng áp dụng **Silence Padding (Chèn khoảng lặng)** theo các mốc Grid (VD: 500ms) để tự động lặp lại đúng vào nhịp gốc (Xem chi tiết thuật toán tại `technical-spec.md`). Không sử dụng Time-Stretch để tránh làm biến dạng âm thanh.
- **Luồng phát nhạc (Zero Latency Sync):**
  - Nhạc nền (BGM) loop liên tục.
  - Mọi AudioClip từ Monster (khi hát) PHẢI được lên lịch phát bằng hệ thống Audio DSP time để đảm bảo beat-snapping chính xác 100%. Nếu một Monster được thả bong bóng giữa nhịp, nó sẽ đợi đến vạch nhịp (beat) tiếp theo để bắt đầu phát, tuyệt đối không được lệch nhịp.

## 2. Input Definitions
- **Drag & Drop (Monster từ danh sách):**
  - Cuộn dọc danh sách các Monster bên trái.
  - Kéo (Drag) Monster đã mở khóa và thả (Drop) vào các placeholder/slots trên sân khấu.
  - Nếu thả thành công (hoặc đè lên Monster cũ đang hát), hệ thống mở `NewAnimalPopupUI`.
- **NewAnimalPopupUI Input:**
  - Nút Record, Play, Try Again, Confirm. Đếm ngược thời gian ghi âm tự động dừng khi hết thời gian giới hạn.
- **Thu thập tiền (Tap):**
  - Raycast 2D vào các Object Tiền đang rớt để thu thập.

## 3. Entity Behaviours
- **Monster:**
  - Nhận AudioClip. Nếu có Voice Filter (Pitch Up, Robot), áp dụng hiệu ứng AudioMixer tương ứng lên Source của nó.
  - Liên kết Animation: Khi Audio Source bắt đầu phát (Singing), kích hoạt `Anim_Singing`. Khi dừng phát, trở về `Anim_Idle`.
  - Animation `Singing` cần phản hồi theo biên độ âm lượng (Audio Spectrum). Âm thanh lớn mồm mở to / scale to.

## 4. Economy Logic & Formulas
- **Cơ chế rớt tiền (Auto Drop):** 
  - Cứ mỗi khi BGM hoàn thành 1 chu kỳ loop (ví dụ: 4 Bars = 1 Loop), NẾU trên sân khấu có ít nhất 1 Monster đang hát, hệ thống sẽ rớt ra `X` đồng xu.
  - Công thức `X` = `Số lượng Monster đang hát` * `Hệ số rớt tiền cơ bản` (Mặc định = 1).
- **Cơ chế vật lý đồng xu:** Xu sinh ra tại vị trí Monster, rớt theo lực Gravity 2D có nảy nhẹ (Bouncing), nằm trên một collider ở đáy màn hình.

## 5. Third-Party / OS Requirements
- Cấp quyền Microphone cho iOS/Android.
- Tích hợp Ads (Rewarded Video) để mở khóa nội dung.
