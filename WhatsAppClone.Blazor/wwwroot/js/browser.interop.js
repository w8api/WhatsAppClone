var masterLayoutInstance = null, shortcuts = [];

window.browserFunctions = {
    _componentsInstances: {},
    setInstance: function (className, instance) {
        window.browserFunctions._componentsInstances[className] = instance;
    },
    call: async function (className, functionName, ...args) {
        await window.browserFunctions._componentsInstances[className].invokeMethodAsync(functionName, args);
    },
    download: function (filename, base64) {
        var link = document.createElement('a');
        link.download = filename;
        link.href = base64;
        document.body.appendChild(link); // Needed for Firefox
        link.click();
        document.body.removeChild(link);
    },
    consoleLog: function (message) {
        console.log(message);
    },
    consoleError: function (message) {
        console.error(message);
    },
    localStorageSet: function (key, value) {
        window.localStorage.setItem(key, value);
    },
    localStorageGet: function (key) {
        return window.localStorage[key];
    },
    localStorageRemove: function (key) {
        return window.localStorage.removeItem(key);
    },
    setCookie: function (name, value, expireDate, domain) {
        var expires = expireDate ? "; expires=" + new Date(expireDate) : "";
        domain = domain ? "; domain=" + domain : "";
        document.cookie = name + "=" + value + expires + domain + "; path=/";
    },
    getCookie: function (c_name) {
        if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf(c_name + "=");
            if (c_start != -1) {
                c_start = c_start + c_name.length + 1;
                c_end = document.cookie.indexOf(";", c_start);
                if (c_end == -1) {
                    c_end = document.cookie.length;
                }
                return unescape(document.cookie.substring(c_start, c_end));
            }
        }
        return "";
    },
    deleteCookie: function (name) {
        document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    },
    setMasterLayoutInstance: function (instance) {
        masterLayoutInstance = instance;
    },
    historyBack: function (delta) {
        if (!delta) history.back();
        else history.go(delta);
    },
    alert: function (message) {
        alert(message);
    },
    addShortcut: function (key, modifier, id) {
        if (key != null) {
            shortcuts.push({ key: key, modifier: modifier, id });
        }
    },
    removeShortcut: function (id) {
        if (id != null) {
            shortcuts = shortcuts.filter(function (e) { return e.id != id; });
        }
    },
    setWindowTitle: function (title) {
        document.title = title;
    },
    windowLocation: function (url, seAbreNoBrowserQuandoElectron) {
        if (seAbreNoBrowserQuandoElectron === true && typeof require !== 'undefined') {
            require('electron').shell.openExternal(url);
            return;
        }

        window.location = url;
    },
    playSound: function (sound) {
        if (sound != null) {
            var audio = new Audio(sound);
            audio.play();
        } else {
            // http://marcgg.com/blog/2016/11/01/javascript-audio/
            try {
                window.AudioContext = window.AudioContext || window.webkitAudioContext;
                var context = new AudioContext();
                var o = context.createOscillator();
                var g = context.createGain();
                o.type = 'sine';
                o.connect(g);
                o.frequency.value = 830.6;
                g.connect(context.destination);
                o.start(0);
                g.gain.exponentialRampToValueAtTime(0.00001, context.currentTime + 1.5);
            } catch (e) {
                console.log(e);
            }
        }
    }
};

$(function () {
    document.addEventListener('keydown', function (e) {

        //// when modal opened, disable
        //if (modalInstance != null) {
        //    return;
        //}

        // initialize shortcuts
        if (!shortcuts.length)
            return;

        var modifier = 0, keyCode = e.which || e.keyCode, key = e.key;

        if (key == null) return;

        if (e.altKey) {
            modifier = 1;
        } else if (e.ctrlKey) {
            modifier = 2;
        } else if (e.shiftKey) {
            modifier = 4
        }

        for (var i = 0; i < shortcuts.length; i++) {
            var v = shortcuts[i];

            if (
                (
                    (v.key.length == 1 && v.key.charCodeAt(0) == keyCode)
                    || v.key.toLowerCase() == key.toLowerCase()
                )
                && v.modifier == modifier
            ) {
                e.preventDefault();
                masterLayoutInstance.invokeMethodAsync('OnShortcut', keyCode, key, modifier);
                break;
            }
        }
    });
});