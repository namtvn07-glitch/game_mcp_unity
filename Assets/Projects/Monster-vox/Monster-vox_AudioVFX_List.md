# Monster-vox Audio & VFX List

## 1. Background Music (BGM) (10 Themes)
Tất cả BGM phải được thiết kế dạng loop hoàn hảo, định dạng WAV/OGG để tối ưu bộ nhớ, có BPM rõ ràng để phục vụ thuật toán Auto-Quantize.
- **`BGM_HomeMenu`:** Âm nhạc nền cho sảnh chính (Chill, hơi ma mị, không có lời).
- **`BGM_Theme_01`:** (Spooky Room) Trống kick mạnh, nhiều khoảng trống dải trung/cao. BPM: 100.
- **`BGM_Theme_02`:** (Neon Graveyard) Bass điện tử, Cyberpunk-spooky. BPM: 120.
- **`BGM_Theme_03`:** (Alien Stage) Nhạc Synthwave ngoài hành tinh. BPM: 110.
- **`BGM_Theme_04`:** (Pumpkin Patch) Giai điệu Halloween vui nhộn, lốc cốc. BPM: 95.
- **`BGM_Theme_05`:** (Haunted Forest) Trống mộc (Tribal beat) rùng rợn. BPM: 105.
- **`BGM_Theme_06`:** (Vampire Castle) Nhạc giao hưởng (Organ/Harpsichord) nhịp điệu. BPM: 115.
- **`BGM_Theme_07`:** (Cyber Dystopia) Trống Dubstep xé tai. BPM: 140.
- **`BGM_Theme_08`:** (Ghost Ship) Điệu hải tặc, tiếng accordion đứt quãng. BPM: 125.
- **`BGM_Theme_09`:** (Zombie Mall) Nhạc nền siêu thị cổ điển bị chập chờn (Vaporwave). BPM: 90.
- **`BGM_Theme_10`:** (Hellish Disco) Nhạc Disco nhịp điệu cực dồn dập. BPM: 130.

## 2. Sound Effects (SFX)
- **UI SFX:**
  - `SFX_ButtonTap`: Tiếng click xương xẩu hoặc tiếng thạch lạch cạch.
  - `SFX_PanelOpen`: Tiếng rít cửa gỗ hỏng hoặc tiếng "woosh" quỷ dị.
  - `SFX_Error`: Tiếng còi "è" hoặc tiếng ợ báo lỗi (khi không đủ xu).
- **Gameplay SFX:**
  - `SFX_RecordStart`: Tiếng "Bíp" hoặc tiếng hít hơi ngắn.
  - `SFX_RecordStop`: Tiếng "Póp" hoặc tiếng băng cassette dừng.
  - `SFX_DragBubble`: Tiếng "poing poing" khi kéo thả bong bóng.
  - `SFX_DropSuccess`: Tiếng nuốt chửng hoặc cắn rộp khi gán audio cho quái.
  - `SFX_CoinDrop`: Tiếng đồng xu rơi leng keng xuèng xuống sàn.
  - `SFX_CoinCollect`: Tiếng "bling" chói tai, tươi sáng.
  - `SFX_MonsterSpawn`: Tiếng "Pouf" xuất hiện.
  - `SFX_Unlock`: Tiếng tèn ten vỡ òa chúc mừng.

## 3. Visual Effects (VFX)
Sử dụng Particle System 2D.
- **`VFX_CoinDrop`:** Hiệu ứng bắn xu cong dạng Parabol từ vị trí Monster xuống đất.
- **`VFX_CoinCollect`:** Hiệu ứng nổ sáng lấp lánh (Sparkle) tại điểm chạm nhặt xu.
- **`VFX_RecordPulse`:** Hiệu ứng vòng tròn lan tỏa (Ripple) xung quanh nút Record màu đỏ khi đang thu âm, thay đổi độ lớn theo cường độ mic.
- **`VFX_MonsterSingWave`:** Các nốt nhạc hoặc lượn sóng (Soundwaves) nhỏ bay ra từ miệng Monster khi hát.
- **`VFX_SmokePoof`:** Hiệu ứng khói tím nổ ra khi Monster xuất hiện hoặc biến mất.
- **`VFX_UnlockConfetti`:** Pháo giấy dạng xương/kẹo nổ ra từ giữa màn hình khi mở khóa Theme/Monster mới.
