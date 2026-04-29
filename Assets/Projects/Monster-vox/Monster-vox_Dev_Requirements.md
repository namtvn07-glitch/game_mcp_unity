# Monster-vox Dev Requirements

*(Lưu ý: Tài liệu này định nghĩa logic và luật chơi. Developers tự quyết định architecture, singletons, thư viện và tên script).*

## 1. Core Logic Rules & Flow
- **Game States:** Game chỉ cần 2 trạng thái chính: `HomeState` và `StageState`.
- **Luồng dữ liệu thu âm (Microphone):** 
  - Khi người chơi giữ nút Record (tối đa 2 giây), thiết bị bắt đầu lưu data vào một Audio Buffer.
  - Khi thả nút, buffer này phải được phân tích: Cắt bỏ khoảng lặng (Trim Silence) ở đầu và cuối.
  - Sau đó, audio phải được **ép nhịp (Time-Stretch)** hoặc **chuẩn hóa độ dài** sao cho khớp đúng 1 Bar (hoặc 1/2 Bar) của BPM nhạc nền hiện tại. Không làm méo pitch nếu chỉ giãn nhịp thông thường.
- **Luồng phát nhạc (Zero Latency Sync):**
  - Nhạc nền (BGM) loop liên tục.
  - Mọi AudioClip từ Monster (khi hát) PHẢI được lên lịch phát bằng hệ thống Audio DSP time để đảm bảo beat-snapping chính xác 100%. Nếu một Monster được thả bong bóng giữa nhịp, nó sẽ đợi đến vạch nhịp (beat) tiếp theo để bắt đầu phát, tuyệt đối không được lệch nhịp.

## 2. Input Definitions
- **Nút Record (Hold):** Cần nhận diện sự kiện `PointerDown` và `PointerUp`.
- **Drag & Drop (Bong bóng âm thanh):**
  - Kéo bong bóng sinh ra từ nút Record.
  - Phát hiện Drop: Raycast 2D vào khu vực màn hình. Nếu thả trúng Collider của một Slot/Monster đang trống (hoặc thay thế con cũ), gán AudioClip cho Monster đó.
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
- Tính năng Quay Màn Hình In-game (bao gồm Audio In-game và cả Audio từ Microphone đang phát).
- Native Share để share video mp4 lên TikTok/Mạng xã hội.
- Tích hợp IAP (Mua xu) và Ads (Rewarded Video).
