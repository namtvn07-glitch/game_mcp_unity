# Monster-vox UX Wireframes & Flow

## 1. UX Flowchart (Mermaid)

```mermaid
graph TD
    %% Define Styles
    classDef screen fill:#2C3E50,stroke:#34495E,stroke-width:2px,color:#ECF0F1;
    classDef popup fill:#8E44AD,stroke:#9B59B6,stroke-width:2px,color:#FFF;
    classDef action fill:#E67E22,stroke:#D35400,stroke-width:1px,color:#FFF;

    %% Nodes
    Init((Start)) --> Home[UI_Screen_MainMenu]
    
    Home -->|Click Play Theme| Stage[UI_Screen_Stage]
    Home -->|Click Store| Store[UI_Popup_Store]
    Home -->|Click Settings| Settings[UI_Popup_Settings]
    
    Stage -->|Hold Record| Record((Recording Audio))
    Record -->|Release| SpawnBubble((Spawn Bubble))
    SpawnBubble -->|Drag & Drop| Stage
    Stage -->|Click Back| Home
    
    Store -->|Buy Item| CheckCoin{Enough Coin?}
    CheckCoin -->|Yes| UnlockItem((Unlock Item))
    CheckCoin -->|No| Store
    Store -->|Close| Home
    
    Settings -->|Close| Home

    %% Assign Styles
    class Home,Stage screen;
    class Store,Settings popup;
    class Record,SpawnBubble,UnlockItem action;
```

## 2. ASCII Wireframes

### Màn hình Sảnh Chính (`UI_Screen_MainMenu`)
```text
+---------------------------------------+
|                 HOME                  |
|                                       |
|  [Coin_Icon] 1500      [Settings_Btn] |
|---------------------------------------|
|                                       |
|                                       |
|          <   [ Theme 1 ]   >          |
|                (Image)                |
|                                       |
|                                       |
|           +---------------+           |
|           |  PLAY THEME   |           |
|           +---------------+           |
|                                       |
|                                       |
|---------------------------------------|
|  [ Btn_Store ]      [ Btn_Collection ]|
+---------------------------------------+
```

### Màn hình Sân Khấu (`UI_Screen_Stage`)
```text
+---------------------------------------+
|                 STAGE                 |
|                                       |
|  [Btn_Back]    (0 Coins)   [Btn_Rec]  |
|---------------------------------------|
|                                       |
|                                       |
|                                       |
|                                       |
|     [Slot 1]    [Slot 2]    [Slot 3]  |
|      (Mon)       (Mon)       (Mon)    |
|                                       |
|                                       |
|                                       |
|             (Sound Bubble)            |
|           +---------------+           |
|           |  HOLD RECORD  |           |
|           +---------------+           |
+---------------------------------------+
```

### Popup Cửa Hàng (`UI_Popup_Store`)
```text
+---------------------------------------+
|  +---------------------------------+  |
|  |             STORE         [X]   |  |
|  |---------------------------------|  |
|  | [Themes] [Monsters] [Slots]     |  |
|  |---------------------------------|  |
|  |                                 |  |
|  |  +-------+           +-------+  |  |
|  |  | Th_1  |           | Th_2  |  |  |
|  |  | [0 C] |           | [Lock]|  |  |
|  |  +-------+           +-------+  |  |
|  |                                 |  |
|  |  +-------+           +-------+  |  |
|  |  | Mon_2 |           | Mon_3 |  |  |
|  |  | [150] |           | [300] |  |  |
|  |  +-------+           +-------+  |  |
|  |                                 |  |
|  |---------------------------------|  |
|  |  [ IAP: Premium No Ads $2.99 ]  |  |
|  +---------------------------------+  |
+---------------------------------------+
```

### Popup Cài Đặt (`UI_Popup_Settings`)
```text
+---------------------------------------+
|  +---------------------------------+  |
|  |            SETTINGS       [X]   |  |
|  |---------------------------------|  |
|  |                                 |  |
|  |  BGM:    [========|      ]      |  |
|  |                                 |  |
|  |  SFX:    [===========|   ]      |  |
|  |                                 |  |
|  |  Vocal:  [=====|         ]      |  |
|  |                                 |  |
|  |---------------------------------|  |
|  |      [ Restore Purchases ]      |  |
|  |                                 |  |
|  |         [ Tutorial ]            |  |
|  |                                 |  |
|  +---------------------------------+  |
+---------------------------------------+
```
