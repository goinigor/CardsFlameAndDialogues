using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.Dialogues
{
    public class DialogueAvatarsCache : IDisposable
    {
        private struct IconsData
        {
            public Texture2D texture;
            public Sprite sprite;
        }
        
        private readonly API _api;

        public DialogueAvatarsCache(API api)
        {
            _api = api;
        }
        
        private Dictionary<string, IconsData> _avatars = new Dictionary<string, IconsData>();

        public async UniTask<Sprite> GetAvatar(AvatarData avatarData, CancellationToken token)
        {
            if (_avatars.TryGetValue(avatarData.name, out var avatar))
            {
                return avatar.sprite;
            }

            var downloadedAvatar = await _api.DownloadTexture2D(avatarData.url, token);
            if (token.IsCancellationRequested || downloadedAvatar == null)
                return null;
            
            var createdSprite = Sprite.Create(downloadedAvatar, new Rect(0, 0, downloadedAvatar.width, downloadedAvatar.height), Vector2.zero);
            _avatars.Add(avatarData.name, new IconsData { texture = downloadedAvatar, sprite = createdSprite });
            
            return createdSprite;
        }

        public void Dispose()
        {
            foreach (var avatar in _avatars)
            {
                UnityEngine.Object.Destroy(avatar.Value.texture);
                UnityEngine.Object.Destroy(avatar.Value.sprite);
            }
            
            _avatars.Clear();
        }
    }
}