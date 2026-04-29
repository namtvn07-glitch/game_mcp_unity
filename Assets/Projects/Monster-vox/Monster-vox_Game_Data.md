# Monster-vox Game Data

Tài liệu này chứa các dữ liệu số học, giá trị và thông số cân bằng cho hệ thống kinh tế (Economy) của MVP Prototype.

## 1. Currency Generation (Công thức sinh tiền)
- **Chu kỳ sinh tiền:** Rớt mỗi khi BGM phát xong 1 Loop (VD: 8 giây).
- **Công thức:** `(Số lượng Monster trên stage) * Base_Drop`
- **Base_Drop:** 1 Spooky Coin.
- **Giới hạn rơi:** Không giới hạn, miễn là game vẫn mở màn hình Sân khấu.
- **Ad Reward (Nhận XU):** Xem 1 quảng cáo Rewarded Video = 100 Spooky Coins.
- **Ad Reward (Mở Item):** Người chơi có thể chọn xem quảng cáo (Ads) thay cho trả xu để mở khóa Theme/Monster.

## 2. Themes Data (10 Themes)
Danh sách các Theme. Theme mặc định (Theme_01) được mở sẵn. Người chơi có thể dùng Xu hoặc xem Ads để mở.

| Theme ID | Name | Background Asset | BGM Asset | Unlock Cost |
| :--- | :--- | :--- | :--- | :--- |
| `Theme_01` | Spooky Room | `BG_Theme_01` | `BGM_Theme_01` | Mở sẵn |
| `Theme_02` | Neon Graveyard | `BG_Theme_02` | `BGM_Theme_02` | 500 Xu / 2 Ads |
| `Theme_03` | Alien Stage | `BG_Theme_03` | `BGM_Theme_03` | 800 Xu / 3 Ads |
| `Theme_04` | Pumpkin Patch | `BG_Theme_04` | `BGM_Theme_04` | 1000 Xu / 4 Ads |
| `Theme_05` | Haunted Forest | `BG_Theme_05` | `BGM_Theme_05` | 1500 Xu / 5 Ads |
| `Theme_06` | Vampire Castle | `BG_Theme_06` | `BGM_Theme_06` | 2000 Xu / 6 Ads |
| `Theme_07` | Cyber Dystopia | `BG_Theme_07` | `BGM_Theme_07` | 2500 Xu / 7 Ads |
| `Theme_08` | Ghost Ship | `BG_Theme_08` | `BGM_Theme_08` | 3000 Xu / 8 Ads |
| `Theme_09` | Zombie Mall | `BG_Theme_09` | `BGM_Theme_09` | 3500 Xu / 9 Ads |
| `Theme_10` | Hellish Disco | `BG_Theme_10` | `BGM_Theme_10` | 4000 Xu / 10 Ads |

## 3. Monsters Data (20 Monsters)
Danh sách quái vật. Quái đầu tiên được mở sẵn.

| Monster ID | Name | Base Asset | Voice Filter | Unlock Cost |
| :--- | :--- | :--- | :--- | :--- |
| `Mon_01` | Normal Cyclops | `Monster_01` | `None` (Normal) | Mở sẵn |
| `Mon_02` | Chipmunk Ghost | `Monster_02` | `Pitch-Up` | 150 Xu / 1 Ad |
| `Mon_03` | Robo Bat | `Monster_03` | `Robot` | 300 Xu / 1 Ad |
| `Mon_04` | Deep Blob | `Monster_04` | `Pitch-Down` | 400 Xu / 2 Ads |
| `Mon_05` | Echo Skeleton | `Monster_05` | `Echo/Delay` | 500 Xu / 2 Ads |
| `Mon_06` | Alien Soprano | `Monster_06` | `Pitch-Up High` | 600 Xu / 2 Ads |
| `Mon_07` | Glitch Demon | `Monster_07` | `Bitcrusher` | 700 Xu / 3 Ads |
| `Mon_08` | Chorus Mummy | `Monster_08` | `Chorus` | 800 Xu / 3 Ads |
| `Mon_09` | Reverb Zombie | `Monster_09` | `Reverb Heavy`| 900 Xu / 3 Ads |
| `Mon_10` | Auto-Tune Orc | `Monster_10` | `Auto-Tune` | 1000 Xu / 4 Ads |
| `Mon_11` | Flanger Spider | `Monster_11` | `Flanger` | 1100 Xu / 4 Ads |
| `Mon_12` | Phaser Pumpk | `Monster_12` | `Phaser` | 1200 Xu / 4 Ads |
| `Mon_13` | Bass Wolf | `Monster_13` | `Bass Boost` | 1300 Xu / 5 Ads |
| `Mon_14` | Tremolo Witch | `Monster_14` | `Tremolo` | 1400 Xu / 5 Ads |
| `Mon_15` | Squeaky Rat | `Monster_15` | `Pitch-Up Max`| 1500 Xu / 5 Ads |
| `Mon_16` | Growl Bear | `Monster_16` | `Distortion` | 1600 Xu / 6 Ads |
| `Mon_17` | Radio Frank | `Monster_17` | `Radio EQ` | 1700 Xu / 6 Ads |
| `Mon_18` | Alien Bass | `Monster_18` | `Sub-Bass` | 1800 Xu / 6 Ads |
| `Mon_19` | Spectral Cat | `Monster_19` | `Reverse FX` | 1900 Xu / 7 Ads |
| `Mon_20` | Mega Boss | `Monster_20` | `Multi-Layer` | 2000 Xu / 8 Ads |

## 4. Slots Expansion Data (Tối đa 5 Slots)
Người chơi bắt đầu với 3 Slots trên sân khấu.

| Slot Level | Total Slots | Unlock Cost |
| :--- | :--- | :--- |
| 1 | 3 Slots | Mở sẵn |
| 2 | 4 Slots | 500 Xu / 2 Ads |
| 3 | 5 Slots (Max) | 1500 Xu / 5 Ads |

## 5. IAP Data (In-App Purchases)
Giá trị Hard Currency thực tế.

| Product ID | Description | Reward | Price (USD) |
| :--- | :--- | :--- | :--- |
| `iap_premium` | Gói No-Ads + Mở full Theme/Monster | Remove Ads, Unlock All | $4.99 |
| `iap_coin_small` | Gói Xu Nhỏ | 1000 Coins | $0.99 |
| `iap_coin_large` | Gói Xu Lớn | 5000 Coins | $3.99 |
