# Monster-vox Art Requirements

## 1. Visual Style Guidelines
- **Art Style:** 2D "Spooky-Derpy" (Rùng rợn nhưng ngớ ngẩn). Nét vẽ hoạt hình, hơi méo mó/thô ráp.
- **Color Palette:** Tông tối làm chủ đạo (Đen, Tím than, Xanh rêu) kết hợp với các điểm nhấn phản quang (Neon Green, Neon Blue, Hot Pink).
- **Vibe:** Dễ thương, hài hước một cách quái đản, không gây sợ hãi thực sự.

## 2. Environment & Props Asset List (10 Themes)
| Asset Name | Type | Description / Notes |
| :--- | :--- | :--- |
| `BG_HomeMenu` | 2D Background | Cánh cửa kỳ bí hoặc giao diện playlist tối tăm, mờ ảo. |
| `BG_Theme_01` | 2D Background | Căn phòng ma ám cơ bản, có mạng nhện và ánh nến. |
| `BG_Theme_02` | 2D Background | Nghĩa địa với ánh sáng neon xanh/tím. |
| `BG_Theme_03` | 2D Background | Sân khấu ngoài hành tinh với phi thuyền UFO. |
| `BG_Theme_04` | 2D Background | Cánh đồng bí ngô ma quái Halloween. |
| `BG_Theme_05` | 2D Background | Rừng ma mị với cây cối cong queo. |
| `BG_Theme_06` | 2D Background | Bên trong lâu đài Ma Cà Rồng với rèm đỏ tía. |
| `BG_Theme_07` | 2D Background | Thành phố Cyberpunk bỏ hoang (Cyber Dystopia). |
| `BG_Theme_08` | 2D Background | Boong tàu cướp biển ma lênh đênh. |
| `BG_Theme_09` | 2D Background | Siêu thị Zombie với kệ hàng đổ nát. |
| `BG_Theme_10` | 2D Background | Sàn nhảy Disco địa ngục với đèn xoay lửa. |
| `Prop_StageSlot_01..10`| 2D Prop | Thiết kế 10 loại bục đứng tương ứng với 10 Theme. |

## 3. Character (Monster) Asset List & Animations (20 Monsters)
*Tất cả quái vật cần được thiết kế 2D tách lớp (Spine/Sprite) để có thể diễn hoạt theo 4 State: `Idle`, `Singing`, `Spawn`, `Happy`.*

| Monster ID | Monster Base Design |
| :--- | :--- |
| `Mon_01` | Normal Cyclops (Một mắt béo lùn) |
| `Mon_02` | Chipmunk Ghost (Bóng ma nhỏ xíu) |
| `Mon_03` | Robo Bat (Dơi cơ khí) |
| `Mon_04` | Deep Blob (Cục nhầy khổng lồ) |
| `Mon_05` | Echo Skeleton (Bộ xương gõ nhịp) |
| `Mon_06` | Alien Soprano (Người ngoài hành tinh cổ dài) |
| `Mon_07` | Glitch Demon (Ác quỷ bị lỗi hình ảnh nhiễu sóng) |
| `Mon_08` | Chorus Mummy (Xác ướp quấn băng) |
| `Mon_09` | Reverb Zombie (Zombie há mồm to) |
| `Mon_10` | Auto-Tune Orc (Orc cầm mic vàng) |
| `Mon_11` | Flanger Spider (Nhện 6 mắt) |
| `Mon_12` | Phaser Pumpk (Đầu bí ngô phát sáng) |
| `Mon_13` | Bass Wolf (Người sói tai cụp) |
| `Mon_14` | Tremolo Witch (Phù thủy cưỡi chổi) |
| `Mon_15` | Squeaky Rat (Chuột cống đột biến) |
| `Mon_16` | Growl Bear (Gấu sẹo khâu) |
| `Mon_17` | Radio Frank (Frankenstein cắm ăng ten) |
| `Mon_18` | Alien Bass (UFO mini có loa bass) |
| `Mon_19` | Spectral Cat (Mèo âm hồn 3 đuôi) |
| `Mon_20` | Mega Boss (Quỷ Satan lùn mặc vest) |

## 4. UI Elements Asset List
| UI Asset Name | Description | State / Variants |
| :--- | :--- | :--- |
| `Icon_Record` | Biểu tượng Microphone | Trạng thái: Bình thường, Đang giữ (Nhấp nháy đỏ) |
| `Icon_Coin_Spooky` | Đồng xu Spooky | Dùng làm Icon và cả Physics Prop thả xuống sàn |
| `Icon_Play` | Nút Play ở sảnh | Xanh Neon nổi bật |
| `Icon_Back` | Mũi tên quay lại |  |
| `Icon_Store` | Biểu tượng Cửa hàng |  |
| `Icon_Settings` | Bánh răng Cài đặt |  |
| `Icon_ScreenRecord` | Camera/Video | Trạng thái: Bình thường, Đang quay (Đỏ) |
| `Icon_Lock` | Ổ khóa | Dùng đè lên Theme/Monster chưa mua |
| `Icon_AdPlay` | Nút xem Video Ads | Biểu tượng Play tam giác |
| `Item_SoundBubble` | Bong bóng âm thanh lơ lửng | Có hiệu ứng sóng âm ở trong |
| `UI_Frame_Avatar` | Khung chân dung Monster | Dùng trong Store |
| `UI_Bar_Beat` | Thanh hiển thị nhịp độ |  |

## 5. VFX Textures
- `Tex_SmokeParticle` (Cho khói khi quái vật Spawn)
- `Tex_SparkleParticle` (Cho hiệu ứng nhặt xu)
- `Tex_SoundWaveParticle` (Cho nốt nhạc bay ra từ miệng)
- `Tex_ConfettiParticle` (Pháo giấy xương sọ cho lúc unlock)
