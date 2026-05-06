# Review Ngày 2026-05-05: Gỡ lỗi toàn diện MVP Monster-vox, Nâng cấp UI và Xử lý Economy

Hôm nay là một ngày dài tập trung vào việc nối lại tất cả các mảng ghép của dự án Monster-vox để ra được một MVP chơi được hoàn chỉnh. Chúng ta đã đi từ việc thống nhất lại tài liệu thiết kế, gỡ lỗi logic gameplay cốt lõi, nâng cấp toàn bộ hệ thống UI, cho đến việc fix các luồng kinh tế sinh tiền của quái vật.

Dưới đây là đúc kết lại toàn bộ quá trình, những lựa chọn đã đưa ra và bài học xương máu (đặc biệt hữu ích để không lặp lại lỗi cũ).

---

### 1. Cách tiếp cận và Lý luận (Approach and Reasoning)

Khởi đầu ngày hôm nay, dự án ở trạng thái có nhiều tính năng bị phân mảnh và một số mâu thuẫn trong tài liệu (như In-App Purchase hay Quay màn hình không cần thiết cho MVP). 
**Cách tiếp cận của chúng ta là "Từ Xương đến Da":**
- **Xương (Tài liệu & Kiến trúc):** Cắt bỏ ngay lập tức những tính năng thừa để tránh bị nhiễu. Thống nhất lại tài liệu thiết kế (GDD) để Dev và Art cùng nhìn về một hướng.
- **Cơ bắp (Gameplay Logic):** Giải quyết các core loop bị lỗi: Kéo thả bong bóng cho quái ăn, quái hát theo nhịp (Quantization) và sinh ra tiền.
- **Da thịt (UI & Assets):** Bổ sung các Asset UI còn thiếu (được xử lý hàng loạt bằng AI/Script để cắt viền, xóa nền) và nâng cấp hệ thống text để giao diện sắc nét hơn.

Chúng ta cố gắng giải quyết triệt để từng "nút thắt" thay vì làm mọi thứ dang dở.

### 2. Những con đường không đi (Roads Not Taken)

*Đây là phần quan trọng nhất - tại sao chúng ta không chọn những cách dễ hơn?*

- **Giữ lại UI Text cũ (UnityEngine.UI.Text):** 
  Đã có lúc định giữ lại UI Text mặc định cho nhanh, nhưng chúng ta quyết định chuyển toàn bộ sang `TextMeshPro`. Lý do: Text cũ scale rất mờ và thiếu các tính năng styling cao cấp. Đổi lại, chúng ta phải viết một Editor Tool để migrate hàng loạt, nhưng sự đánh đổi này mang lại chất lượng hình ảnh sắc nét và độ ổn định lâu dài.
- **Thêm Collider bằng tay cho quái vật:** 
  Khi phát hiện lỗi hitbox lúc kéo bong bóng, thay vì mở từng prefab quái vật ra để tinh chỉnh BoxCollider (cực kỳ mất thời gian và dễ sai khi Art đổi hình), chúng ta đã dùng tiện ích `SpriteColliderGenerator` để tự động tạo hitbox dựa trên hình dạng thật của sprite lúc runtime.
- **Xử lý va chạm kéo thả bằng UI Raycast:** 
  Vì bong bóng là đối tượng giao thoa giữa UI và thế giới 2D, chúng ta đã chọn dùng `Physics2D.OverlapPoint` thay vì Raycast của UI. Nó giúp quản lý chính xác hơn việc bong bóng đang chạm vào layer "Monster" nào trong thế giới game.

### 3. Các mảnh ghép kết nối với nhau ra sao (How the Pieces Connect)

Mọi thứ trong dự án này (đặc biệt là phần sinh tiền) đều dựa vào **Event-Driven Architecture (Kiến trúc hướng sự kiện)**. Hãy tưởng tượng nó như một dây chuyền nhà máy:
1. `MonsterController` là quản lý, nó ra lệnh cho `QuantizedAudioPlayer` bắt đầu căn nhịp để hát.
2. Khi `QuantizedAudioPlayer` hoàn thành một vòng lặp âm thanh, nó "hét lên" (phát event).
3. `StageManager` nghe thấy tiếng hét đó, bèn gọi `CoinSpawner` rớt ra một đồng xu.
4. Khi đồng xu được thu thập, `EconomyManager` tăng số dư tiền và lại "hét lên" (phát event thay đổi tiền).
5. Cuối cùng, `HUDCoinDisplay` (anh chàng UI) nghe thấy event thay đổi tiền, bèn cập nhật con số TextMeshPro hiển thị.

Hôm nay dây chuyền này bị đứt ở công đoạn cuối (Tiền tăng nhưng UI không đổi) do vấn đề về thời điểm đăng ký lắng nghe sự kiện (Race condition giữa Awake/Start).

### 4. Công cụ và Phương pháp (Tools and Methods)

- **Editor Tool Scripting:** Dùng code C# chạy trong Editor để tự động chuyển đổi Text sang TextMeshPro. Cứu tinh của những thao tác click chuột lặp đi lặp lại.
- **Action / Event của C#:** Sử dụng `Action<int>` để làm các trạm phát thanh, giúp UI và Logic tách biệt hoàn toàn (Decoupling).

### 5. Sự đánh đổi (Tradeoffs)

- **Tự động hóa vs Làm tay:** Viết một cái tool migrate TextMeshPro mất khoảng 30 phút, trong khi sửa tay có thể chỉ mất 15 phút cho số lượng UI hiện tại. NHƯNG, tool đó sẽ dùng được mãi mãi về sau nếu UI mở rộng thêm. Đầu tư thời gian vào Tooling luôn là sự đánh đổi có lãi.
- **Decoupling (Tách rời) vs Trực quan:** Code theo kiểu Event (Event-driven) khiến bạn khó Ctrl+Click (Go to Definition) để biết ngay ai đang gọi ai, vì mã nguồn không reference trực tiếp đến nhau. Phải đổi lại bằng việc đọc log và hiểu luồng dữ liệu, nhưng bù lại xóa một component UI đi game vẫn không bị crash.

### 6. Sai lầm và Ngõ cụt (Mistakes and Dead Ends)

**Sự cố ngớ ngẩn nhất:**
Gọi trực tiếp vào component con thay vì qua Controller wrapper. 
Cụ thể, có lúc chúng ta đã gọi thẳng các hàm của `QuantizedAudioPlayer` thay vì bảo `MonsterController` làm việc đó. Hậu quả là `MonsterController` không biết rằng quái vật đang hát, các biến state (`isSinging`) không được cập nhật, kéo theo các logic khác (như sinh tiền) bị tê liệt ngầm mà không hề báo lỗi đỏ (Error) trên console.
*Cách giải quyết:* Luôn giao tiếp qua class cấp cao nhất của một Object (Wrapper/Controller).

### 7. Cạm bẫy tương lai (Future Pitfalls)

*Ghim lại những điều này vào não, bạn sẽ cảm ơn tôi sau:*
- **Đừng dùng `Time.timeScale = 0f` để pause bừa bãi:** Nếu bạn pause game kiểu này để hiện UI, mọi hiệu ứng âm thanh, coroutines mới sinh ra sẽ "đóng băng". Hãy dùng `.mute` cho Audio và `unscaledDeltaTime` cho UI animation.
- **Nút bấm và Lambda trong Editor Script:** Nếu bạn code Editor tool và dùng `button.onClick.AddListener(() => {...})`, lúc chạy game nút đó sẽ vô dụng. Hành động đó không được serialize vào Scene. Phải gán reference qua `SerializedObject` hoặc chạy hàm đó ở `Awake/Start` lúc runtime.
- **Kẻ thù rò rỉ bộ nhớ (Memory Leak):** Đã `+=` (subscribe) vào một Event, thì bắt buộc phải có `-=` (unsubscribe) ở hàm `OnDisable` hoặc `OnDestroy`. Rất nhiều lỗi bóng ma (gọi hàm 2, 3 lần) xuất phát từ việc quên dòng này.

### 8. Góc nhìn của Chuyên gia vs Người mới (Expert vs Beginner Eye)

- **Khi UI tiền không cập nhật:**
  - *Beginner:* Sẽ đi tìm cái script cộng tiền, cố gắng FindObjectOfType cái HUD UI rồi gọi thẳng hàm `HUD.UpdateText()` vào đó. Xong! Rất nhanh, nhưng sẽ tạo ra "Code thảm họa" (Spaghetti code) vì Logic và UI dính chặt vào nhau.
  - *Expert:* Sẽ nhìn ngay vào **Vòng đời khởi tạo (Lifecycle)**. Có phải UI subscribe event muộn hơn lúc event được bắn ra? Có phải do load Scene không đồng bộ? Họ sẽ sửa bằng cách dọn dẹp lại thứ tự `Awake/Start` và tuân thủ chặt chẽ mô hình Publisher - Subscriber.

### 9. Bài học mang theo (Transferable Lessons)

Dù bạn làm game này, game khác hay làm App, web, hãy nhớ 2 nguyên tắc vàng hôm nay:
1. **DRY (Don't Repeat Yourself) trên cả thao tác:** Đừng chỉ DRY trong code, hãy DRY trong cả lúc thao tác Editor. Nếu phải click chuột 5 lần để làm một việc giống nhau, hãy viết tool tự động làm nó.
2. **UI chỉ là kẻ hóng chuyện:** Giao diện chỉ nên lắng nghe dữ liệu và hiển thị. Không bao giờ cho phép UI chứa logic tính toán tiền bạc hay máu me của nhân vật. Tách bạch Data và View là chìa khóa để dự án lớn lên không bị sụp đổ.
