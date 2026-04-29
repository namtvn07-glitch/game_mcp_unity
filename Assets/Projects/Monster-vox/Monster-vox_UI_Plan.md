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
  - `Btn_ScreenRecord`: Nút quay màn hình (Đổi state khi đang quay).
- **Center Area (Gameplay HUD):**
  - Không có nút tĩnh, là không gian cho `Prop_StageSlot` và vật lý rơi đồng xu.
- **Bottom Bar:**
  - `Btn_Record_Hold`: Nút thu âm khổng lồ ở giữa. Giữ để thu âm.
  - `Item_SoundBubble`: Bong bóng sinh ra trên nút Record sau khi thả tay.

## 3. Popup Cửa Hàng (`UI_Popup_Store`)
- **Header:**
  - Tiêu đề "STORE".
  - `Btn_CloseStore`: Đóng popup.
- **Tabs:** 
  - `Tab_Themes`, `Tab_Monsters`, `Tab_Slots`, `Tab_Coins`.
- **Content Panel (Grid Layout):**
  - Các ô Item bán (Hiển thị Avatar, Tên, Giá).
  - `Btn_Buy_Coin`: Nút mua bằng xu (chứa giá text).
  - `Btn_Buy_Ad`: Nút xem quảng cáo để nhận (có icon Play).
- **IAP Banners:**
  - `Btn_IAP_Premium`: Nút mua gói No Ads.

## 4. Popup Cài Đặt (`UI_Popup_Settings`)
- **Header:** "SETTINGS" và `Btn_CloseSettings`.
- **Content:**
  - `Slider_Volume_BGM`: Chỉnh nhạc nền.
  - `Slider_Volume_SFX`: Chỉnh âm thanh hiệu ứng.
  - `Slider_Volume_Vocal`: Chỉnh âm lượng hát của quái vật.
  - `Btn_RestorePurchases`: Khôi phục IAP.
  - `Btn_Tutorial`: Xem lại hướng dẫn.
