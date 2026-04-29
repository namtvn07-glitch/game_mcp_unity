# Monster-vox Integration Map

## 1. [SYSTEM_HOOKS]

### Trigger: `OnNavigateToStage(ThemeData)`
- **Dev:** `GameManager.ChangeState(StageState)`, load `ThemeData`.
- **Art/Anim:** Load Background Sprite (e.g., `BG_Theme1_SpookyRoom`). Load `Prop_StageSlot` tương ứng. Các Monster có sẵn trên sân khấu chạy `Anim_Spawn`.
- **VFX:** Chạy `VFX_SmokePoof` tại các vị trí Slot.
- **SFX:** Phát BGM của Theme (e.g., `BGM_Theme1_Base`), phát `SFX_MonsterSpawn`, dừng `BGM_HomeMenu`.
- **UI:** Ẩn `UI_Screen_MainMenu`, hiện `UI_Screen_Stage` (với Nút Record, HUD Coin, Nút Quay màn hình).

### Trigger: `OnMicrophoneRecordStart`
- **Dev:** `MicrophoneRecorder.StartRecording()`.
- **Art/Anim:** Không đổi.
- **VFX:** Bật `VFX_RecordPulse` quanh nút Record `Btn_Record_Hold`, độ lớn phụ thuộc biên độ âm thanh đầu vào.
- **SFX:** Phát `SFX_RecordStart`.
- **UI:** Nút `Icon_Record` nhấp nháy đỏ.

### Trigger: `OnMicrophoneRecordStop`
- **Dev:** `MicrophoneRecorder.StopRecording()`. `AutoQuantizeEngine` xử lý audio (Trim, Stretch/Quantize). Khởi tạo Object Sound Bubble.
- **Art/Anim:** Không đổi.
- **VFX:** Tắt `VFX_RecordPulse`.
- **SFX:** Phát `SFX_RecordStop`.
- **UI:** Nút Record trở lại trạng thái bình thường. Sinh ra UI item `Item_SoundBubble` lơ lửng chờ kéo thả.

### Trigger: `OnDragDropBubbleSuccess` (Thả vào Slot/Monster)
- **Dev:** Raycast xác nhận thả trúng `Prop_StageSlot`. Truyền AudioClip vào Monster đó. Đặt lịch phát Audio dựa trên DSP Time đồng bộ với BGM loop.
- **Art/Anim:** Monster chuyển trạng thái sang `Anim_Singing`.
- **VFX:** Bật `VFX_MonsterSingWave` tại miệng Monster.
- **SFX:** Phát `SFX_DropSuccess`.
- **UI:** Xóa `Item_SoundBubble` khỏi ngón tay.

### Trigger: `OnEconomyAutoDrop` (BGM hoàn thành 1 chu kỳ)
- **Dev:** Tính toán số tiền: `X = Monsters * Base_Drop`. Rớt xu dùng 2D Physics Gravity.
- **Art/Anim:** Monster nhún nhẹ hoặc thả đồng xu (`Icon_Coin_Spooky`).
- **VFX:** Áp dụng lực đẩy (AddForce) tạo hiệu ứng parabol (`VFX_CoinDrop`).
- **SFX:** Phát `SFX_CoinDrop` khi xu sinh ra.
- **UI:** Cập nhật gián tiếp, xu rớt trong không gian World.

### Trigger: `OnCoinCollect` (Tap vào xu)
- **Dev:** Xóa object xu. Cộng tiền vào `PlayerData`.
- **Art/Anim:** Monster hiển thị `Anim_Happy` nhẹ.
- **VFX:** Chạy `VFX_CoinCollect` (Sparkle).
- **SFX:** Phát `SFX_CoinCollect`.
- **UI:** `HUD_SessionCoins` tăng số lên.

### Trigger: `OnUnlockItem` (Mua Theme/Monster/Slot)
- **Dev:** Kiểm tra `Game_Data.md` xem đủ tiền không. Trừ tiền, cập nhật Save file.
- **Art/Anim:** Nếu đang ở Store, gỡ bỏ `Icon_Lock` khỏi ảnh đại diện của Item.
- **VFX:** Bắn `VFX_UnlockConfetti` tại vị trí trung tâm.
- **SFX:** Phát `SFX_Unlock`.
- **UI:** Trừ số lượng ở `HUD_CoinDisplay`. Cập nhật nút bấm thành "Đã Sở Hữu".

## 2. [SCENE_HIERARCHY]
Cấu trúc MainScene (Single-Scene Architecture):

```text
MonsterVox_MainScene
├── ⚙️ Core_Managers (DontDestroyOnLoad)
│   ├── GameManager
│   ├── AudioManager (Chứa Master AudioMixer để xử lý Vocal Filters)
│   ├── EconomyManager
│   └── InputManager
├── 🎵 AudioSources
│   ├── Source_BGM
│   ├── Source_SFX
│   └── Audio_Mixer_Groups (Master, BGM, SFX, Vocal)
├── 🌍 Environment_2D
│   ├── Background_Renderer
│   ├── StageSlots_Group (Tối đa 5 Slots)
│   └── DropArea_Coins (Chứa physics collider chắn xu rơi)
├── 👻 Entities
│   └── MonsterPool (Spawn các Monster 2D)
└── 🖥️ UI_Canvas (Screen Space)
    ├── Panel_MainMenu (Home Screen)
    ├── Panel_Stage (Nút Record, Session Coins HUD)
    ├── Popup_Store
    ├── Popup_Settings
    ├── Item_SoundBubble_DragLayer
    └── VFX_UI_Overlay (Confetti)
```
