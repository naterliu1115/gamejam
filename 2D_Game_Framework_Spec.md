## 2D 橫軸像素遊戲 開發框架規格書

### 一、專案初始化與結構

1. **Unity 專案設置**

   * Unity 版本：6.x
   * 專案模板：2D
   * 目錄結構：

     ```
     /Assets
       /Scenes
         - Main.unity
         - Level1.unity
       /Scripts
         /Core
         /Player
         /Interactable
         /Rescue
         /Enemies
         /Boss
         /UI
       /Prefabs
       /Art
         /Sprites
         /Tilemaps
       /Audio
         /SFX
         /Music
     ```
2. **場景設定**

   * Main.unity：GameManager、UI Canvas、InputManager
   * LevelX.unity：Tilemap、關卡物件擺放

### 二、核心模組與腳本

| 模組       | 類別                   | 功能描述                                         |
| -------- | -------------------- | -------------------------------------------- |
| Input 管理 | `InputManager`       | 接收按鍵長短按、發射事件（Jump/DropDown/Push/Kick/Rescue） |
| 遊戲管理     | `GameManager`        | 關卡流程、成功/失敗判定、分數計算                            |
| 玩家控制     | `PlayerController`   | 移動、跳躍、壁跳、下穿平台、狀態機切換                          |
| 互動物件     | `InteractableObject` | 推動、破壞、生成掉落物                                  |
| 救援系統     | `RescueTarget`       | NPC 跟隨、停留、切換                                 |
| 傷害機制     | `HarmfulObstacle`    | 暈眩／扣血效果觸發                                    |
| 小怪 AI    | `MinorEnemy`         | 巡邏行為、被踢擊死亡                                   |
| Boss 控制  | `BossController`     | 階段機關生成、暈眩機制                                  |
| UI 管理    | `UIManager`          | HUD 顯示、提示文字                                  |

### 三、事件與介面定義

* **InputManager** 事件：

  ```csharp
  public event Action OnJump;
  public event Action OnDropDown;
  public event Action OnPushStart;
  public event Action OnPushEnd;
  public event Action OnKick;
  public event Action OnKickHold;
  public event Action OnRescue;
  ```
* **InteractableObject** 介面：

  ```csharp
  public interface IInteractable {
    void OnPush(Vector2 dir);
    void OnDestroyInteractable();
  }
  ```
* **RescueTarget** 事件：

  ```csharp
  public event Action<RescueTarget> OnFollowStarted;
  public event Action<RescueTarget> OnStay;
  ```
* **HarmfulObstacle** 介面：

  ```csharp
  public interface IHarmful {
    void ApplyEffect(PlayerController player);
  }
  ```

### 四、Prefab 規範

| 名稱                | 組件                                          | 說明         |
| ----------------- | ------------------------------------------- | ---------- |
| Player            | Rigidbody2D, Collider2D, PlayerController   | 主角         |
| Platform          | TilemapCollider2D, PlatformEffector2D       | 單向通行/可下穿平台 |
| Box\_Push         | Rigidbody2D, Collider2D, InteractableObject | 可推／可破壞方塊   |
| Box\_DestructOnly | Collider2D, InteractableObject              | 只能破壞方塊     |
| NPC\_Rescue       | Collider2D (Trigger), RescueTarget          | 拯救目標       |
| Enemy\_Minor      | Rigidbody2D, Collider2D, MinorEnemy         | 巡邏小怪       |
| Boss              | Rigidbody2D, Collider2D, BossController     | 關卡 Boss    |
| Hazard\_Fire      | Collider2D (Trigger), HarmfulObstacle       | 火焰陷阱       |
| Pickup\_Stun      | Collider2D (Trigger), PickupItem            | 撞擊暈眩       |
| Pickup\_Damage    | Collider2D (Trigger), PickupItem            | 撞擊扣血       |

### 五、開發工作清單（程式對接順序）

1. 建立 `InputManager` → 測試事件發射
2. 實作 `PlayerController` → 移動／跳躍／壁跳測試
3. 製作 `InteractableObject` → 推動／破壞測試
4. 完成 `RescueTarget` → 跟隨／停留測試
5. 實作 `HarmfulObstacle` & `PickupItem` → 暈眩／扣血測試
6. 製作 `MinorEnemy` → 巡邏與死亡測試
7. 實作 `BossController` → 機關生成／暈眩測試
8. 整合 `UIManager` → HUD 顯示測試
9. 在 Level1 擺放 Prefabs → 通關流程測試

### 六、企劃開發流程

1. **核心玩法確認**：確立「推、破、跳、壁跳、救援」互動
2. **互動模組規範**：細化物件狀態與接口定義
3. **事件與接口協商**：生成對接文檔
4. **原型驗證**：程式端實作並測試基本互動
5. **關卡設計與測試**：設計謎題並收集反饋
6. **互動迭代**：優化手感、動畫、音效
7. **視覺音效整合**：匯入最終素材
8. **最終驗證**：修復 Bug、版本凍結

### 七、與現有專案整合方案

1. **保留核心架構**
   * 保留 Simulation 事件系統作為核心架構
   * 擴展 PlatformerModel 以支持新功能
   * 維持 KinematicObject 作為物理系統基礎

2. **輸入系統改進**
   * 創建獨立的 InputManager 類別，從 PlayerController 分離輸入處理
   * 實現規格中定義的事件系統
   * 保持與 Unity 新 Input System 的兼容性

3. **擴展玩家控制器**
   * 在現有 PlayerController 基礎上添加壁跳功能
   * 添加下穿平台功能
   * 使用新的 InputManager 替代直接輸入處理

4. **互動物件系統**
   * 創建 IInteractable 介面和 InteractableObject 基類
   * 實現可推動和可破壞物件
   * 整合到現有的物理系統中

5. **救援系統**
   * 創建 RescueTarget 類別實現 NPC 跟隨功能
   * 添加到 Simulation 事件系統中
   * 與 GameController 整合以實現關卡成功條件

6. **傷害機制**
   * 創建 IHarmful 介面和 HarmfulObstacle 類別
   * 與現有 Health 系統整合
   * 實現暈眩和扣血效果

7. **敵人系統擴展**
   * 基於現有 EnemyController 創建 MinorEnemy 類別
   * 添加被踢擊死亡功能
   * 保留現有的巡邏路徑系統

8. **Boss 系統**
   * 創建 BossController 類別
   * 實現階段性機關生成和暈眩機制
   * 與現有事件系統整合

9. **擴展遊戲管理**
   * 擴展 GameController 添加關卡成功/失敗判定
   * 添加分數計算系統
   * 整合救援目標追蹤

10. **UI 系統整合**
    * 擴展現有 UI 系統顯示生命值、分數、時間等
    * 添加提示文字系統
    * 保持與 MetaGameController 的兼容性

### 八、關卡成敗判定與額外加分

#### 成功判定

* 全部拯救目標 (HP > 0) 到達安置點。

#### 失敗判定

* 玩家 HP 歸 0。
* 剩餘時間用盡。
* 所有拯救目標死亡。

#### 額外加分條件

* 擊倒地圖內小怪。
* 破壞地圖內可破壞障礙物。
* 通關剩餘時間。

```csharp
// GameManager 範例
if (rescuedCount == totalRescueTargets && player.HP > 0) OnLevelSuccess?.Invoke();
if (player.HP <= 0 || timeRemaining <= 0f || aliveRescueTargets == 0) OnLevelFail?.Invoke();
void CalculateBonus() {
    score += defeatedMinorEnemies * minorEnemyScore;
    score += destroyedObjects * objectDestroyScore;
    score += Mathf.CeilToInt(timeRemaining) * timeBonus;
}
```
