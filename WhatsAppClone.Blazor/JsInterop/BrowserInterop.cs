using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor.JsInterop
{
    public class BrowserInterop : BaseInterop 
    {
        #region Enum

        public enum ModifierKeys : int
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4
        }

        #endregion

        const string jsName = "browserFunctions";

        #region Vars

        bool? _isMobile;
        string _navigatorUserAgent;
        readonly List<(string shortcutKey, ModifierKeys shortcutModifier, Func<Task> callback, Guid id)> _shortcutCallbacks = new ();

        #endregion

        public BrowserInterop(IJSRuntime jsRuntime)
            : base(jsRuntime)
        { }

        /// <summary>
        /// Play some sound
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        /// <remarks>http://marcgg.com/blog/2016/11/01/javascript-audio/</remarks>
        public async ValueTask PlaySound(string sound = "sine")
            => await JsRuntime.InvokeVoidAsync($"{jsName}.playSound", sound);

        /// <summary>
        /// Set the instance of component to enable calls js -> blazor
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async ValueTask SetComponentInstance(string fullTypeName, object instance)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.setInstance", fullTypeName, instance);

        /// <summary>
        /// download the base64
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="base64"></param>
        /// <returns></returns>
        public async ValueTask Download(string fileName, string base64)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.download", fileName, base64);

        /// <summary>
        /// Log something on developer tools
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async ValueTask ConsoleLog(string message)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.consoleLog", message);

        /// <summary>
        /// Log something on developer tools
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async ValueTask ConsoleError(string message)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.consoleError", message);

        /// <summary>
        /// Returns cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async ValueTask<string> CookieGet(string key)
            => await JsRuntime.InvokeAsync<string>($"{jsName}.getCookie", key);

        /// <summary>
        /// Change the cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireDate"></param>
        /// <param name="domain">Domain of the cookie</param>
        /// <returns></returns>
        public async ValueTask CookieSet(string key, string value, DateTime? expireDate, string domain = null)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.setCookie", key, value, expireDate, domain);

        /// <summary>
        /// Remove the cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async ValueTask CookieDelete(string key)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.deleteCookie", key);

        /// <summary>
        /// change local staorage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="serializa">if serializes the value</param>
        /// <returns></returns>
        public async ValueTask LocalStorageSet<T>(string key, T value, bool serializa = true)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.localStorageSet", key, serializa ? JsonSerializer.Serialize(value) : value as string);

        /// <summary>
        /// Remove the key from local storage
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async ValueTask LocalStorageRemove(string key)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.localStorageRemove", key);

        /// <summary>
        /// Get something from local storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="deserializa">if need to deserialize</param>
        /// <returns></returns>
        public async ValueTask<T> LocalStorageGet<T>(string key, bool deserializa = true)
        {
            T res = default;
            var json = await JsRuntime.InvokeAsync<string>($"{jsName}.localStorageGet", key);

            if (!string.IsNullOrWhiteSpace(json))
            {
                res = deserializa ? JsonSerializer.Deserialize<T>(json) : (T)Convert.ChangeType(json, typeof(T));
            }

            return res;
        }

        /// <summary>
        /// Get something from local storage
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public async ValueTask<object> LocalStorageGet(string key, Type valueType)
        {
            object res = default;
            var json = await JsRuntime.InvokeAsync<string>($"{jsName}.localStorageGet", key);

            if (!string.IsNullOrWhiteSpace(json))
            {
                res = JsonSerializer.Deserialize(json, valueType);
            }

            return res;
        }

        public async ValueTask SetWindowTitle(string title) =>
            await JsRuntime.InvokeVoidAsync($"{jsName}.setWindowTitle", title);

        /// <summary>
        /// Redirect the user to some URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ifOpenInBrowserWhenElectron"></param>
        /// <returns></returns>
        public async ValueTask WindowLocation(string url, bool ifOpenInBrowserWhenElectron = false) =>
            await JsRuntime.InvokeVoidAsync($"{jsName}.windowLocation", url, ifOpenInBrowserWhenElectron);

        /// <summary>
        /// History back of browser
        /// </summary>
        /// <param name="delta">How muck backs to do</param>
        /// <returns></returns>
        public async ValueTask HistoryBack(int delta = -1) =>
            await JsRuntime.InvokeVoidAsync($"{jsName}.historyBack", delta);

        public async ValueTask Alert(string message) =>
            await JsRuntime.InvokeVoidAsync($"{jsName}.alert", message);

        public async ValueTask<Guid> AddShortcut(string shortcutKey, ModifierKeys shortcutModifier, Func<Task> callback)
        {
            var id = Guid.NewGuid();
            await JsRuntime.InvokeVoidAsync($"{jsName}.addShortcut", shortcutKey, shortcutModifier, id);
            _shortcutCallbacks.Add((shortcutKey, shortcutModifier, callback, id));
            return id;
        }

        public async ValueTask RemoveShortcut(Guid id)
        {
            await JsRuntime.InvokeVoidAsync($"{jsName}.removeShortcut", id);
            _shortcutCallbacks.RemoveAll(w => w.id == id);
        }

        /// <summary>
        /// The browser user agent
        /// </summary>
        /// <returns></returns>
        public async ValueTask<string> NavigatorUserAgent()
        {
            if (string.IsNullOrWhiteSpace(_navigatorUserAgent))
            {
                _navigatorUserAgent = await JsRuntime.InvokeAsync<string>($"{jsName}.navigatorUserAgent");
            }

            return _navigatorUserAgent;
        }

        /// <summary>
        /// If browser is mobile
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsMobile()
        {
            if (_isMobile == null)
            {
                var navigatorUserAgent = await NavigatorUserAgent();
                var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                _isMobile = b.IsMatch(navigatorUserAgent) || v.IsMatch(navigatorUserAgent.Substring(0, 4));
            }

            return _isMobile.Value;
        }

        /// <summary>
        /// Monitoring the keydown event
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        internal async Task<bool> PerformShortcut(decimal keyCode, string key, ModifierKeys modifier)
        {
            bool isShortcutExecuted = false;

            foreach (var (shortcutKey, shortcutModifier, callback, id) in _shortcutCallbacks
                .Where(w =>
                    !string.IsNullOrWhiteSpace(w.shortcutKey)
                    &&
                    (
                        (char.TryParse(w.shortcutKey, out char charRes) && charRes == keyCode)
                        || w.shortcutKey.ToLower() == key.ToLower()
                    )
                    && w.shortcutModifier == modifier
                )
                .ToList() // necessário pois os atalhos podem ocorrer de forma mto rápida e dar collection modified
            )
            {
                await callback();
                isShortcutExecuted = true;
            }

            return isShortcutExecuted;
        }
    }
}