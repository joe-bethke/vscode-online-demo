/* eslint-disable */

// Copyright (c) Microsoft. All rights reserved.

// Polyfills for cross browser support

if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        return this.substr(position || 0, searchString.length) === searchString;
    };
}

if (!Array.prototype.includes) {
    Object.defineProperty(Array.prototype, "includes", {
        value: function (searchElement, fromIndex) {
            if (this === null)
                throw new TypeError('"this" is null or not defined');

            var o = Object(this);
            var len = o.length >>> 0;
            if (len === 0) return false;

            var n = fromIndex | 0;
            var k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);

            function sameValueZero(x, y) {
                return (
                    x === y ||
                    (typeof x === "number" &&
                        typeof y === "number" &&
                        isNaN(x) &&
                        isNaN(y))
                );
            }

            while (k < len) {
                if (sameValueZero(o[k], searchElement)) return true;
                k++;
            }

            return false;
        },
    });
}

if (!String.prototype.includes) {
    String.prototype.includes = function (search, start) {
        "use strict";
        if (typeof start !== "number") start = 0;

        if (start + search.length > this.length) {
            return false;
        } else {
            return this.indexOf(search, start) !== -1;
        }
    };
}

if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (searchStr, Position) {
        if (!(Position < this.length)) Position = this.length;
        else Position |= 0;
        return (
            this.substr(Position - searchStr.length, searchStr.length) ===
            searchStr
        );
    };
}

if (!String.prototype.padStart) {
    String.prototype.padStart = function padStart(targetLength, padString) {
        targetLength = targetLength >> 0; //floor if number or convert non-number to 0;
        padString = String(padString || " ");
        if (this.length > targetLength) {
            return String(this);
        } else {
            targetLength = targetLength - this.length;
            if (targetLength > padString.length) {
                padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
            }
            return padString.slice(0, targetLength) + String(this);
        }
    };
}

// From https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/entries
if (!Object.entries) {
    Object.entries = function (obj) {
        var ownProps = Object.keys(obj),
            i = ownProps.length,
            resArray = new Array(i); // preallocate the Array
        while (i--) resArray[i] = [ownProps[i], obj[ownProps[i]]];

        return resArray;
    };
}

if (!Object.values) {
    Object.values = (obj) => Object.keys(obj).map((key) => obj[key]);
}

// From https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/find
// https://tc39.github.io/ecma262/#sec-array.prototype.find
if (!Array.prototype.find) {
    Object.defineProperty(Array.prototype, "find", {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this === null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== "function") {
                throw new TypeError("predicate must be a function");
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return kValue.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return kValue;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return undefined.
            return undefined;
        },
        configurable: true,
        writable: true,
    });
}

if (!String.prototype.repeat) {
    String.prototype.repeat = function(count) {
      'use strict';
      if (this == null)
        throw new TypeError('can\'t convert ' + this + ' to object');
  
      var str = '' + this;
      // To convert string to integer.
      count = +count;
      // Check NaN
      if (count != count)
        count = 0;
  
      if (count < 0)
        throw new RangeError('repeat count must be non-negative');
  
      if (count == Infinity)
        throw new RangeError('repeat count must be less than infinity');
  
      count = Math.floor(count);
      if (str.length == 0 || count == 0)
        return '';
  
      // Ensuring count is a 31-bit integer allows us to heavily optimize the
      // main part. But anyway, most current (August 2014) browsers can't handle
      // strings 1 << 28 chars or longer, so:
      if (str.length * count >= 1 << 28)
        throw new RangeError('repeat count must not overflow maximum string size');
  
      var maxCount = str.length * count;
      count = Math.floor(Math.log(count) / Math.log(2));
      while (count) {
         str += str;
         count--;
      }
      str += str.substring(0, maxCount - str.length);
      return str;
    }
  }