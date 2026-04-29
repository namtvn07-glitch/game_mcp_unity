# Monster-vox Dev Architecture

## 1. Core Architecture Pattern
- Áp dụng Singleton pattern cho các Manager cốt lõi.
- Áp dụng Observer (Event-driven) cho hệ thống UI và tính điểm để giảm dependency.

## 2. Core Managers & Scripts
- **GameManager:** Quản lý State của game (Home, Stage_Playing, Stage_Recording).
- **AudioManager:** Quản lý BGM đa luồng, đồng bộ hóa thời gian cực kỳ chính xác (Zero latency, Audio DSP time) để đảm bảo các clip thu âm phát đúng nhịp.
- **MicrophoneRecorder:** Cấp quyền micro, thu âm thanh độ trễ thấp (<2s), lưu vào AudioClip buffer.
- **AutoQuantizeEngine:** Thuật toán phân tích AudioClip, cắt khoảng lặng đầu/cuối, kéo giãn (Time-stretch) hoặc pitch-shift để khớp với BPM của Theme hiện tại.
- **DragDropManager:** Xử lý Raycast 2D, kéo bong bóng âm thanh từ UI thả vào 2D Sprite của Monster.
- **MonsterController:** Nhận AudioClip, kích hoạt Animation (Singing) đồng bộ với việc phát Audio.
- **EconomyManager:** Xử lý logic rớt xu định kỳ, lưu trữ tiền tệ.

## 3. Data Structures & Save States
Sử dụng JSON để lưu trữ dữ liệu người chơi (hoặc PlayerPrefs cho thông tin đơn giản).
- **PlayerData:**
  - `totalCoins` (int)
  - `unlockedThemes` (List<string>)
  - `unlockedMonsters` (List<string>)
  - `unlockedSlots` (int)
- **ThemeData (ScriptableObject):**
  - `themeID`
  - `themeName`
  - `bgmClip`
  - `bpm` (float)
  - `backgroundSprite`
- **MonsterData (ScriptableObject):**
  - `monsterID`
  - `prefab`
  - `voiceFilterType` (Enum: Normal, PitchUp, PitchDown, Robot)
  - `cost` (int)

## 4. Physics & Interaction Requirements
- Hệ thống Physics 2D đơn giản, chủ yếu sử dụng `BoxCollider2D` hoặc `CircleCollider2D` để nhận diện điểm thả (Drop target) cho các Monster Slot và nhận diện Tap để nhặt xu.

## 5. Third-party SDKs
- **Screen Recorder SDK:** NatCorder hoặc Everyplay (đã ngừng) -> Cần tìm plugin ghi màn hình hỗ trợ audio nội bộ tốt trên Mobile.
- **Ads SDK:** Unity Ads hoặc AppLovin MAX (cho Rewarded video).
- **IAP:** Unity IAP.
