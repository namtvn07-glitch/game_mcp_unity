# Hướng Dẫn Sử Dụng: Game Designer Skill (Super-Sub Concept)

## Tổng quan
Skill **`game-designer`** đóng vai trò là một Master Orchestrator (Người Điều Phối Đỉnh Cao). Chuyên chịu trách nhiệm tiếp nhận ý tưởng phát triển Game Mobile mới của bạn và nhào nặn nó thành một bản **Game Design Document (GDD)** tiêu chuẩn.

Kiến trúc Super-Sub Skill độc đáo giúp loại bỏ hoàn toàn tình trạng "tràn bộ nhớ" (Context Loss) của AI, nhờ đó các tiểu tiết từ cốt truyện, luật chơi, cho đến lúc ráp bảng asset luôn khớp nhau tuyệt đối. Hệ thống sẽ bóc tách mọi thứ thành các checklist rõ ràng cho từng phòng ban (Dev, Art, Sound).

## Cách sử dụng cơ bản

Trong khung chat với AI, bạn chỉ cần gọi tên skill kèm theo ý tưởng hạt giống:

```
@/game-designer [Ý_TƯỞNG_GAME_HOẶC_LINK_THAM_KHẢO]
```

---

## Các Ví dụ Tham khảo (Example Prompts)

Tùy vào hoàn cảnh, bạn có thể sử dụng Skill này theo nhiều dạng Input khác nhau:

### Dạng 1: Đưa ra ý tưởng sơ khai (Raw Idea)
Nếu bạn có một ý tưởng nổ ra ngẫu nhiên trong đầu, cứ việc ném vào Skill.
> `@/game-designer Tôi muốn làm một game hypercasual đua tự do. Nhân vật là một viên kẹo dẻo chạy trên đường dây điện, vuốt sang trái phải để nhảy né chim chóc. Mục đích chỉ để leo rank điểm cao nhất.`

### Dạng 2: Tạo Clone/Tách kỹ năng từ game có sẵn (References)
Dùng khi bạn muốn tham khảo một game trên thị trường hoặc từ 1 video tiktok gameplay.
> `@/game-designer Hãy phân tích core loop của trò "Phone Case DIY". Lược qua các tính năng trang trí rườm rà, hãy viết một bản GDD Prototype MVP cốt lõi tập trung hoàn toàn vào kỹ năng "Xịt Sơn" lên ốp lưng để DEV team có thể test độ mượt của vật lý sơn.`

### Dạng 3: Dò trend và Tự động xây dựng (Trend Research)
Nhờ AI tự làm game dựa trên xu hướng thị trường.
> `@/game-designer Hãy duyệt nhanh thị trường xem thể loại giải đố ASMR phân loại ốc vít (Wood Nuts & Bolts) đang có luật chơi cụ thể ra sao. Từ đó dựng ra bản cấu trúc GDD chi tiết cho một game clone tương tự nhưng lấy theme là Sắp Xếp Hành Lý Sân Bay.`

---

## Luồng hoạt động ẩn của AI (Pipeline)
Khi bấm gửi các câu lệnh trên, bạn sẽ thấy AI tiến hành chạy 3 tiểu trình (Sub-skills) tuần tự trước khi đưa ra kết quả cuối:

1. **Phase 1 (Concept - Ý tưởng):** Phát triển Pitch giới thiệu. Liệt kê công khai các tính năng bị gạch bỏ (Discarded) để ngăn chặn dự án bị bôi vẽ (Scope Creep).
2. **Phase 2 (Rules - Luật):** Không bay bổng nghệ thuật, AI ép sát Gameplay thành các luật vật lý và điều kiện thắng/thua thô ráp.
3. **Phase 3 (Matrix - Ma trận):** 
   - Dịch Luật thành 1 Bảng Ma trận Sự Kiện (Event Matrix) chứa toàn bộ Art/VFX/SFX ID. 
   - Đóng gói toàn bộ Nút bấm và Màn hình Menu thành sơ đồ UI Architecture chi tiết.
   - Phân rã nhóm tài nguyên trong danh sách thành các cấu trúc Category độc lập (Static Art, VFX, Sound, UI).

Tiến trình kết thúc bằng thao tác gán tự động **Auto-Export Split**:
- Tạo ra 1 file Master Document `[ProjectName]_GDD.md` hoàn chỉnh lưu thẳng vào gốc thư mục dự án con hiện hành.
- **Tính năng mở rộng:** Băm toàn bộ Checklist Thiết kế thành 4 file Data rác độc lập (`_Art_Assets.json`, `_VFX_Assets.json`, `_Audio_Assets.json`, `_UI_Assets.json`) đi kèm để tích hợp thẳng vào khay chứa của AI Game Artist và Unity Engine.
