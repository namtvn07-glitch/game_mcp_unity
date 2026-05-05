# Monster-vox UI Plan

## 1. Màn hình Sảnh Chính (`UI_Screen_MainMenu`)
- **Top Bar:**
  - `HUD_CoinDisplay`: Text hiển thị số Spooky Coin hiện có, đi kèm `Icon_Coin_Spooky`.
  - `Btn_Settings`: Nút bấm hình bánh răng mở `UI_Popup_Settings`.
- **Center Area (Theme Selection):**
  - `Slider_ThemeCarousel`: Khu vực vuốt ngang để chọn Theme.
  - Hiển thị Image của Theme. Có `Icon_Lock` đè lên nếu chưa mua.
  - `Text_ThemeName`: Tên Theme.
  - `Btn_PlayTheme`: Nút Play bự màu xanh neon.
- **Bottom Bar:**
  - `Btn_Store`: Mở `UI_Popup_Store`.
  - `Btn_Collection`: Xem danh sách quái vật.

## 2. Màn hình Sân Khấu (`UI_Screen_Stage`)
- **Top Bar:**
  - `Btn_BackToHome`: Nút quay lại sảnh.
  - `HUD_SessionCoins`: Bộ đếm số xu nhặt được trong lần chơi này.
- **Left Panel (Monster Selection):**
  - `ScrollList_Monsters`: Danh sách cuộn dọc chứa các Monster.
  - Mỗi Item trong danh sách: Ảnh quái vật, Tình trạng khóa/mở khóa, Nút mở khóa (giá xu/quảng cáo).
  - Có thể kéo thả (Drag & Drop) quái vật đã mở khóa từ danh sách này vào các placeholder trên sân khấu.
- **Center Area (Gameplay HUD):**
  - `Prop_StageSlot` (Placeholders): Các bục đứng để thả quái vật vào.
  - Vật lý rơi đồng xu.
  - `UI_Bar_Beat`: Thanh hiển thị nhịp độ nhạc nền để người chơi dễ canh nhịp (Optional).

## 3. Popup Thu Âm Mới (`NewAnimalPopupUI`)
- **Center Panel:**
  - `Img_Portrait`: Ảnh chân dung quái vật vừa được thả vào slot.
  - `Input_Name`: Cho phép người chơi đặt tên.
  - `Txt_Timer`: Đếm ngược thời gian thu âm.
- **Buttons (Trạng thái trước khi thu âm):**
  - `Btn_Record`: Nút to ở giữa để bắt đầu thu âm (sẽ bị ẩn sau khi thu xong).
- **Buttons (Trạng thái sau khi thu âm):**
  - `Btn_TryAgain`: Thu âm lại.
  - `Btn_Play`: Nghe lại bản ghi.
  - `Btn_Confirm`: Chốt đoạn thu âm và gán cho quái vật để bắt đầu biểu diễn.

## 4. Popup Cửa Hàng (`UI_Popup_Store`)
- **Header:**
  - Tiêu đề "STORE".
  - `Btn_CloseStore`: Đóng popup.
- **Tabs:** 
  - `Tab_Themes`, `Tab_Monsters`, `Tab_Slots`, `Tab_Coins`.
- **Content Panel (Grid Layout):**
  - Các ô Item bán (Hiển thị Avatar, Tên, Giá).
  - `Btn_Buy_Coin`: Nút mua bằng xu (chứa giá text).
  - `Btn_Buy_Ad`: Nút xem quảng cáo để nhận (có icon Play).


## 4. Popup Cài Đặt (`UI_Popup_Settings`)
- **Header:** "SETTINGS" và `Btn_CloseSettings`.
- **Content:**
  - `Slider_Volume_BGM`: Chỉnh nhạc nền.
  - `Slider_Volume_SFX`: Chỉnh âm thanh hiệu ứng.
  - `Slider_Volume_Vocal`: Chỉnh âm lượng hát của quái vật.
  - `Btn_Tutorial`: Xem lại hướng dẫn.
