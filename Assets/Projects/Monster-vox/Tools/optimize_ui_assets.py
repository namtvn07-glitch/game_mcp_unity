import os
import sys
import numpy as np
from PIL import Image
try:
    from rembg import remove
except ImportError:
    pass # rembg might not be installed

sizes = {
    "ui_panel_base.png": 800,
    "ui_btn_large.png": 512,
    "ui_btn_round.png": 200,
    "ui_btn_record.png": 400,
    "ui_slider_track.png": 512,
    "ui_slider_handle.png": 128,
    "ui_icon_coin.png": 200,
    "ui_icon_lock.png": 200,
    "ui_item_soundbubble.png": 256,
    "ui_icon_back.png": 200,
    "ui_icon_settings.png": 200,
    "ui_icon_close.png": 200,
    "ui_icon_store.png": 256,
    "ui_icon_screenrecord.png": 200,
    "ui_icon_collection.png": 200,
    "ui_icon_ad.png": 200,
    "ui_hud_pill.png": 400
}

def optimize_image(filepath, filename):
    print(f"Optimizing {filepath}...")
    
    img = Image.open(filepath).convert("RGBA")
    
    # 1. Tách nền Magenta (Chroma Key) nếu có
    data = np.array(img)
    r, g, b, a = data[:,:,0], data[:,:,1], data[:,:,2], data[:,:,3]
    magenta_mask = (r > 120) & (g < 100) & (b > 120) & (a > 0)
    data[magenta_mask, 3] = 0
    img = Image.fromarray(data)
    
    # 2. Auto-crop viền trong suốt
    bbox = img.getbbox()
    if bbox:
        img = img.crop(bbox)
        
    # 3. Proportional Resize về tỷ lệ tối ưu
    max_dim = sizes.get(filename.lower(), 256) # Mặc định icon mới là 256
    w, h = img.size
    if w > max_dim or h > max_dim:
        if w > h:
            new_w = max_dim
            new_h = int(h * (max_dim / w))
        else:
            new_h = max_dim
            new_w = int(w * (max_dim / h))
        img = img.resize((new_w, new_h), Image.Resampling.LANCZOS)
        
    img.save(filepath)
    print(f"Done: {filename} -> {img.size}")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python optimize_ui_assets.py <path_to_ui_folder>")
        sys.exit(1)
        
    dir_path = sys.argv[1]
    for filename in os.listdir(dir_path):
        if filename.lower().endswith('.png'):
            filepath = os.path.join(dir_path, filename)
            try:
                optimize_image(filepath, filename)
            except Exception as e:
                print(f"Error on {filename}: {e}")
