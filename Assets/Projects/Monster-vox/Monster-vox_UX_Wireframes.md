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
    
    Stage -->|Scroll & Drag Monster| Slot((Drop to Slot))
    Slot -->|Trigger| NewAnimalPopup[UI_Popup_NewAnimal]
    NewAnimalPopup -->|Click Record| Record((Recording Audio))
    Record -->|Timeout/Stop| Confirm((Confirm & Save))
    Confirm -->|Assign to Monster| Stage
    Stage -->|Click Back| Home
    
    Store -->|Buy Item| CheckCoin{Enough Coin?}
    CheckCoin -->|Yes| UnlockItem((Unlock Item))
    CheckCoin -->|No| Store
    Store -->|Close| Home
    
    Settings -->|Close| Home

    %% Assign Styles
    class Home,Stage screen;
    class Store,Settings,NewAnimalPopup popup;
    class Record,Slot,UnlockItem,Confirm action;
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
|  [Btn_Back]                   (0 Coins) |
|---------------------------------------|
| [Mon1] |                              |
| [150$] |                              |
|--------|                              |
| [Mon2] |     [Slot 1]    [Slot 2]     |
| [Unlock]     (Empty)     (Empty)      |
|--------|                              |
| [Mon3] |                              |
| [Open] |     [Slot 3]                 |
|--------|     (Empty)                  |
| [Mon4] |                              |
| [150$] |                              |
+---------------------------------------+
```

### Popup New Animal (`UI_Popup_NewAnimal`)
```text
+---------------------------------------+
|  +---------------------------------+  |
|  |           NEW ANIMAL            |  |
|  |---------------------------------|  |
|  |           [ Portrait ]          |  |
|  |           [ InputName]          |  |
|  |                                 |  |
|  |            Timer 00:02          |  |
|  |                                 |  |
|  |        [  Btn Record  ]         |  |
|  |                                 |  |
|  |   [Try Again] [Play] [Confirm]  |  |
|  +---------------------------------+  |
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
|  |                                 |  |
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
|  |                                 |  |
|  |                                 |  |
|  |         [ Tutorial ]            |  |
|  |                                 |  |
|  +---------------------------------+  |
+---------------------------------------+
```
