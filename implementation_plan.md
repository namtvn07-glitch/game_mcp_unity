# Sequentially Generate Remaining Monster-vox Assets

Dự án Monster-vox yêu cầu một số lượng lớn các asset (19 Monsters còn lại, 11 Backgrounds, 10 Props, 12 UI Elements, 4 VFX Textures). Dựa trên yêu cầu của bạn, chúng ta sẽ tiến hành tạo **lần lượt** (sequentially) từng asset một.

## User Review Required

> [!WARNING]
> Theo đúng workflow `game-art-orchestrator`, mỗi một asset đều bắt buộc phải trải qua **2 vòng duyệt (Human-In-The-Loop)**:
> 1. Duyệt bản phác thảo (Sketch & Silhouette)
> 2. Duyệt bản đổ màu (Flat Colors, Shading) và xuất file (Export)
> 
> Việc tạo toàn bộ danh sách trong một lần chat là không khả thi và vi phạm nguyên tắc kiểm soát chất lượng. Do đó, tôi đề xuất chúng ta sẽ đi theo trình tự: **Giải quyết dứt điểm từng Monster một, sau đó mới chuyển sang loại asset khác.**

## Proposed Execution Plan

Chúng ta sẽ ưu tiên hoàn thành danh sách **Character (Monsters)** trước (vì chúng là trung tâm của game), sau đó là Environment, UI, và VFX.

### Danh sách các Monster cần thực hiện (Phase 1)
- `Mon_02`: Chipmunk Ghost (Bóng ma nhỏ xíu)
- `Mon_03`: Robo Bat (Dơi cơ khí)
- `Mon_04`: Deep Blob (Cục nhầy khổng lồ)
- `Mon_05`: Echo Skeleton (Bộ xương gõ nhịp)
- `Mon_06`: Alien Soprano (Người ngoài hành tinh cổ dài)
- `Mon_07`: Glitch Demon (Ác quỷ bị lỗi hình ảnh nhiễu sóng)
- `Mon_08`: Chorus Mummy (Xác ướp quấn băng)
- `Mon_09`: Reverb Zombie (Zombie há mồm to)
- `Mon_10`: Auto-Tune Orc (Orc cầm mic vàng)
- `Mon_11` đến `Mon_20`: ... (Sẽ thực hiện tuần tự)

## Verification Plan

### Thủ tục trên mỗi Asset
1. Tôi tạo phác thảo line-art -> Bạn duyệt.
2. Tôi tạo bản màu dựa trên line-art -> Bạn duyệt.
3. Tôi tự động export vào thư mục dự án và log vào `Generated_Asset_Catalog.md`.
4. Chúng ta lặp lại chu kỳ cho Asset tiếp theo.
