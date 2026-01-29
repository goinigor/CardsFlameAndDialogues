using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.Dialogues
{
    public class DialogueAvatarsCache : IDisposable
    {
        private readonly API _api;

        public DialogueAvatarsCache(API api)
        {
            _api = api;
        }
        
        private Dictionary<string, Texture2D> _avatars = new Dictionary<string, Texture2D>();

        public async UniTask<Sprite> GetAvatar(AvatarData avatarData, CancellationToken token)
        {
            if (_avatars.TryGetValue(avatarData.name, out var avatar))
            {
                var sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), Vector2.zero);
                return sprite;
            }

            var downloadedAvatar = await _api.DownloadTexture2D(avatarData.url, token);
            if (token.IsCancellationRequested || downloadedAvatar == null)
                return null;
            
            _avatars.Add(avatarData.name, downloadedAvatar);
            var createdSprite = Sprite.Create(downloadedAvatar, new Rect(0, 0, downloadedAvatar.width, downloadedAvatar.height), Vector2.zero);
            return createdSprite;
        }

        public void Dispose()
        {
            foreach (var avatar in _avatars)
            {
                UnityEngine.Object.Destroy(avatar.Value);
            }
            
            _avatars.Clear();
        }
    }
}