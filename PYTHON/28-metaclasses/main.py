"""
28: 메타클래스 — type() 동적 생성, __new__/__init__, 커스텀 메타클래스
"""


# 1) type()으로 클래스 동적 생성
DynamicGreeter = type(
    "DynamicGreeter",  # 클래스 이름
    (object,),         # 베이스 클래스 튜플
    {
        "greeting": "안녕",
        "say": lambda self: f"{self.greeting}, 메타클래스 세계!",
    },
)


# 2) 클래스 생성 과정을 관찰하는 메타클래스
class TraceMeta(type):
    """클래스가 정의될 때 호출되는 훅을 보여줍니다."""
    def __new__(mcls, name, bases, namespace):
        print(f"[TraceMeta.__new__] 클래스 {name} 생성 중 (속성: {sorted(namespace)})")
        return super().__new__(mcls, name, bases, namespace)

    def __init__(cls, name, bases, namespace):
        print(f"[TraceMeta.__init__] 클래스 {name} 초기화 완료")
        super().__init__(name, bases, namespace)


class WithTrace(metaclass=TraceMeta):
    def method(self):
        return "instance method"


# 3) 실용 예제: 속성 검증 규칙을 자동 적용
class PositiveMeta(type):
    """__annotations__로 선언된 속성에 자동 검증 게터를 부여합니다."""
    def __new__(mcls, name, bases, namespace):
        print(f"[PositiveMeta] {name} 클래스 정의 시퀀스 처리")
        for attr, annotation in namespace.get("__annotations__", {}).items():
            if attr.startswith("_") or annotation is not int:
                continue
            private_key = f"_{name}__{attr}"
            if private_key not in namespace:
                namespace[private_key] = namespace.get(attr, 0)
                del namespace[attr]

            def make_getter(key):
                def getter(self):
                    return getattr(self, key)
                return getter

            def make_setter(key):
                def setter(self, value):
                    if value < 0:
                        raise ValueError(f"{key}는 음수일 수 없습니다: {value}")
                    setattr(self, key, value)
                return setter

            namespace[attr] = property(make_getter(private_key), make_setter(private_key))
        return super().__new__(mcls, name, bases, namespace)


class AutoValidated(metaclass=PositiveMeta):
    """이 클래스를 상속한 클래스는 int 속성이 자동으로 음수 검증됩니다."""
    pass


class Temperature(AutoValidated):
    celsius: int = 20


# 4) 클래스 등록 패턴 (레지스트리)
class PluginMeta(type):
    """모든 Plugin 하위 클래스를 자동으로 등록합니다."""
    registry = {}

    def __new__(mcls, name, bases, namespace):
        cls = super().__new__(mcls, name, bases, namespace)
        if bases:  # Base(Plugin) 제외, 실제 플러그인만 등록
            mcls.registry[name] = cls
        return cls


class Plugin(metaclass=PluginMeta):
    pass


class ImagePlugin(Plugin):
    pass


class AudioPlugin(Plugin):
    pass


if __name__ == "__main__":
    print("=== 1) type() 동적 생성 ===")
    g = DynamicGreeter()
    print(f"클래스 이름: {type(g).__name__}, 부모: {DynamicGreeter.__bases__}")
    print(g.say())
    print()

    print("=== 2) TraceMeta 관찰 ===")
    w = WithTrace()
    print("인스턴스 메서드 호출:", w.method())
    print()

    print("=== 3) 속성 검증 메타클래스 ===")
    t = Temperature()
    print(f"기본 온도: {t.celsius}")
    t.celsius = 35
    print(f"변경 후: {t.celsius}")
    try:
        t.celsius = -10  # 검증 규칙이 자동 적용됨
    except ValueError as e:
        print("음수 설정 시도 ->", e)
    print()

    print("=== 4) 레지스트리 패턴 ===")
    print("등록된 플러그인:", list(PluginMeta.registry))
    print()

    print("=== 5) 메타클래스 __new__ vs 인스턴스 __new__ ===")
    print("type의 인스턴스 = 클래스, object의 인스턴스 = 객체")
    print(f"WithTrace는 type의 인스턴스인가? {isinstance(WithTrace, type)}")
    print(f"w는 type의 인스턴스인가? {isinstance(w, type)}")
    print(f"w는 WithTrace의 인스턴스인가? {isinstance(w, WithTrace)}")
