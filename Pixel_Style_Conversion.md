# 像素風格轉換方案

## 一、玩家角色轉換

### 1. 精靈圖片設置

- **過濾模式修改**
  - 將所有角色精靈圖的 Filter Mode 從 "Bilinear" 改為 "Point (no filter)"
  - 路徑：Inspector > Texture Type > Filter Mode > Point (no filter)

- **像素比例調整**
  - 降低 Pixels Per Unit 值（從 100 改為 16-32）
  - 路徑：Inspector > Sprite > Pixels Per Unit

- **網格類型設置**
  - 將 Sprite Mesh Type 設為 "FullRect"
  - 路徑：Inspector > Sprite > Mesh Type > Full Rect

- **紋理壓縮調整**
  - 關閉或降低紋理壓縮
  - 路徑：Inspector > Texture Type > Default > Compression > None

### 2. 碰撞器調整

- **碰撞邊界修改**
  - 調整 BoxCollider2D 大小以匹配新的像素比例
  - 確保碰撞邊界與像素對齊
  - 路徑：Player.prefab > BoxCollider2D > Size
  - 注意：專案使用 KinematicObject 類別進行自定義物理處理，但仍依賴 BoxCollider2D 進行碰撞檢測

- **物理參數調整**
  - 調整 KinematicObject 中的物理參數以匹配像素風格移動
  - 可能需要調整 minGroundNormalY、shellRadius 等參數
  - 路徑：PlayerController 腳本 > KinematicObject 參數

### 3. 動畫設置

- **幀率調整**
  - 降低動畫幀率以匹配像素風格（建議 8-12 fps）
  - 路徑：Animation > PlayerIdle.anim, PlayerRun.anim 等 > Sample Rate

- **過渡方式修改**
  - 修改動畫過渡為即時切換而非平滑過渡
  - 路徑：Animator > Player.controller > Transitions > Settings

## 二、整體專案轉換

### 1. 相機設置

- **像素完美相機**
  - 使用 Unity 6 URP 中的 PixelPerfectCamera 組件
  - 路徑：Main Camera > Add Component > Rendering > Pixel Perfect Camera

- **參數設置**
  - 設置 Assets Per Unit 與精靈圖的 Pixels Per Unit 匹配
  - 設置適合的參考解析度（如 320x180、640x360）
  - 啟用 Pixel Snapping
  - 路徑：PixelPerfectCamera 組件設置面板

### 2. 渲染管線調整

- **抗鋸齒設置**
  - 在 URP 設置中關閉 MSAA
  - 路徑：Project Settings > Quality > Anti Aliasing

- **後處理設置**
  - 使用 URP 的後處理堆疊添加像素化效果
  - 路徑：Volume > Post-processing > Add Override

- **陰影設置**
  - 關閉軟陰影選項
  - 路徑：URP Asset > Shadows > Soft Shadows

### 3. 材質與著色器

- **著色器選擇**
  - 使用 URP 中的 2D 像素著色器
  - 路徑：Material > Shader > Universal Render Pipeline > 2D > Sprite-Lit-Default

- **材質設置**
  - 調整材質設置以支持像素風格
  - 路徑：Material > Properties

### 4. UI 調整

- **字體設置**
  - 使用像素風格字體
  - 路徑：UI Text > Font Asset

- **UI 元素過濾模式**
  - 確保 UI 元素使用 Point 過濾模式
  - 路徑：UI Image > Inspector > Texture Type > Filter Mode

- **對齊設置**
  - 調整 UI 元素對齊像素網格
  - 路徑：UI Elements > Rect Transform > Position

### 5. 環境與背景

- **瓦片地圖設置**
  - 調整瓦片地圖使用 Point 過濾模式
  - 路徑：Tilemap > Sprite Atlas > Filter Mode

- **環境精靈圖設置**
  - 確保所有環境精靈圖使用相同的像素設置
  - 路徑：Environment Sprites > Inspector > Texture Type

## 三、實施步驟

### 第一階段：玩家角色轉換

1. **修改精靈圖設置**
   - 選擇所有角色精靈圖（PlayerIdle.png、PlayerRun.png 等）
   - 在 Inspector 中將 Filter Mode 改為 "Point (no filter)"
   - 調整 Pixels Per Unit 為較小的值（如 16-32）
   - 應用更改

2. **調整 Player.prefab**
   - 更新 BoxCollider2D 大小以匹配新的像素比例
   - 調整 SpriteRenderer 中的設置
   - 可能需要修改 KinematicObject 組件中的物理參數

3. **修改動畫設置**
   - 調整所有玩家動畫的幀率
   - 確保動畫使用離散的關鍵幀

### 第二階段：相機與渲染設置

1. **添加像素完美相機**
   - 在主相機上添加 URP 的 PixelPerfectCamera 組件
   - 設置適合的參數

2. **調整 URP 設置**
   - 修改 URP Asset 設置以支持像素風格
   - 關閉抗鋸齒和軟陰影

### 第三階段：全局資源調整

1. **調整所有精靈圖**
   - 為所有遊戲精靈圖應用相同的像素設置
   - 確保一致的 Pixels Per Unit 值

2. **調整 UI 元素**
   - 修改 UI 圖像使用 Point 過濾模式
   - 調整 UI 元素對齊像素網格

## 四、效果對比

| 項目 | 轉換前 | 轉換後 |
|------|--------|--------|
| 精靈過濾模式 | Bilinear (平滑) | Point (像素化) |
| 像素比例 | 100 pixels/unit | 16-32 pixels/unit |
| 相機設置 | 標準相機 | 像素完美相機 |
| 整體視覺效果 | 平滑、漸變 | 鮮明、像素化 |
| 動畫過渡 | 平滑過渡 | 即時切換 |

## 五、注意事項

- 所有精靈圖應使用統一的 Pixels Per Unit 值
- 確保所有視覺元素保持一致的像素密度
- 轉換後可能需要調整遊戲中的移動速度和跳躍高度
- 像素風格通常需要較低的動畫幀率來獲得最佳效果
- 由於專案使用自定義的 KinematicObject 物理系統，需要特別注意調整其參數以適應像素風格
- 確保 BoxCollider2D 的大小和位置與視覺效果一致
